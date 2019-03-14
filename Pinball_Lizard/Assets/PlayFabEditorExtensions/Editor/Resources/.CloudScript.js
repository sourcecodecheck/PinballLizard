

/***** handler *****/

/**
* Update player score and associated experience/level
* @param {Object} args
* @param {String} args.score
* @param {String} args.bestCombo
* @param {String/Number} args.bugsEaten
* @param {Boolean} args.isDailyChallenge
* @param {Boolean} args.victory
* @return {Object} player statistics
*/
handlers.submitScore = function( args ) {
    let newLevel;
    let stats;
    let consecutiveWins;
    let consecutiveLosses;
    let rewards;
    if( args && args.hasOwnProperty( 'score' ) ) {
        // max statistic value, signed 32 bit int
        if( args.score < CONSTS.MAX_STATISTIC_VALUE ) {
            [ newLevel, stats ] = helpers.updatePlayerScore( args.score, args.bestCombo, args.isDailyChallenge );
            stats = helpers.calculateConsecutives( stats, args.isVictory );
            rewards = helpers.calculateRewards( stats, newLevel );
        } else {
            return helpers.createReturnErrorObject( LANG.ERR_MAX_VALUE );
        }
        if( args.hasOwnProperty( 'bugsEaten' ) ) {
            helpers.updatePlayerBugsEaten( args.bugsEaten );
        }
        let result = helpers.getStandardReturnObject();
        result.Rewards = rewards || [];
        return result;
    } else {
        return helpers.createReturnErrorObject( LANG.ERR_SCORE_REQUIRED );
    }
}

/**
* Retrieve player statistics: level, score, experience
* @return {Object} player statistics
*/
handlers.getPlayerStatistics = function() {
    return helpers.getStandardReturnObject();
}

/**
* Initialize player level, score, and experience.
* @return {Object} player statistics
*/
handlers.initializePlayer = function() {
    helpers.initializePlayerStatistics();
    return true;
}

/**
* Retrieve experience requirements for next (x) levels
* @return {Object} requirements
*/
handlers.getUpcomingExperienceRequirements = function( args ) {
    const inputError = helpers.getInputError( args, [ 'level', 'count' ] );
    if( inputError ) {
        return helpers.createReturnErrorObject( inputError );
    }
    const stats = helpers.getPlayerStats();
    const level = stats[ CONSTS.STATISTIC_NAMES.level ].value;
    const max = args.level + args.count + 1;
    const requirements = {};
    for( let i = args.level + 1; i < max; i++ ) {
        requirements[ i ] =  helpers.determineLevelExpRequirement( i );
    }
    return { requirements }
}

/**
* Retrieve very large random number
* @return {Object} { result:<num> }
*/
handlers.getChallengeSeed = function() {
    const helperResult = helpers.getDailyChallengeSeed();
    return { result: helperResult || -1 };
}

/**
* Detect event time
* @param {Object} args
* @param {int} args.timeZoneOffset timezone offset as positive or negative
* @return {Object} { isEventTime:<bool> }
*/
handlers.isEventTime = function( args ) {
    const inputError = helpers.getInputError( args, [ 'timeZoneOffset' ] );
    if( inputError ) {
        return helpers.createReturnErrorObject( inputError );
    }
    return { isEventTime: helpers.isDailyChallenge( args.timeZoneOffset ) };
}

/**
* Get player first login time
* @return {Object} { created: <string> }
*/
handlers.getFirstLogin = function() {
    return helpers.getUserFirstLoginTime();
}

/**
* Remove animosity
* @param {Object} args
* @param {int} args.animosity animosity to subtract
* @return {Object} { result:<boolean> }
*/
handlers.removeAnimosity = function( args ) {
    const inputError = helpers.getInputError( args, [ 'animosity' ] );
    if( inputError ) {
        return helpers.createReturnErrorObject( inputError );
    }
    return { result: helpers.removeAnimosity( args.animosity ) };
}


/***** constants *****/

const CONSTS = {

    STATISTIC_NAMES: {
        score: 'Best Score',
        level: 'Level',
        experience: 'Experience',
        bugs: 'Bugs Eaten',
        dailyChallenge: 'Daily Challenge',
        bestCombo: 'Best Combo',
        consecutiveLosses: 'Consecutive Losses',
        consecutiveWins: 'Consecutive Wins'
    },

    CURRENCY_NAMES: {
        mayhem: 'MH',
        animosity: 'AN',
        gluttony: 'GL',
        bugBucks: 'BB'
    },

    INTERNAL_DATA_KEYS: {
      // starts at midnight, this day
      challengeStart: "gluttonyEventStartDay",
      // ends at midnight, this day (12:00am) this is effectively the 'day after'
      challengeEnd: "gluttonyEventEndDay",
      // numeric seed for daily challenge generation
      challengeSeed: "dailyChallengeSeed",
      // stores last change of daily challenge level, updated each time level changes, timestamp
      challengeLastRegenTime: "dailyChallengeRegenTime"
    },

    TITLE_DATA_KEYS: {
      // the hour of each day (UTC) in which the daily challenge level changes
      challengeLevelRegenHour: "dailyChallengeLevelRegenHour",
      // multiplier for amount of experience required at each level ( l=c*sqrt(e) )
      experienceGrowthConstant: "experienceGrowthConstant",
      // experience multiple for incoming scores (Score -> +Experience)
      experienceScoreMultiple: "experienceScoreMultiple",
      // multiplier for adding gluttony based on incoming score (Events)
      gluttonyMultipler: "gluttonyMultipler",
      // multiplier for adding mayhem based on incoming score
      mayhemMultiplier: "mayhemMultiplier",
      // rewards granted to player on level up (JSON)
      levelUpRewards: "levelUpRewards",
      // rewards granted after N losses
      failureRewards: "rewardAfterNFailures",
      // failures until failure reward
      failuresBeforeRewards: "numFailsBeforeReward"
    },


    // maximum allowed value for statistics
    MAX_STATISTIC_VALUE: Math.pow( 2, 31 )

}


/***** lang *****/

const LANG = {

    ERR_SCORE_REQUIRED: "argument object must contain key: score",
    ERR_REQUIRED: "is required",
    ERR_NO_ARGS: "missing arguments",
    ERR_MAX_VALUE: "maximum value: 2,147,483,647"

}


/***** helpers *****/

const helpers = {

    getStandardReturnObject: function() {
        return helpers.createReturnObject({
            Statistics: helpers.getPlayerStats()
        });
    },

    createReturnErrorObject: function( errorMessage ) {
        return { errorMessage }
    },

    createReturnObject: function( data ) {
        data[ 'errorMessage' ] = '';
        return data;
    },

    createRequest: function( data = {} ) {
        data[ 'PlayFabId' ] = currentPlayerId;
        return data;
    },

    createExperience: function( stats, score = 0 ) {
        let experience = stats[ CONSTS.STATISTIC_NAMES.experience ] || { Value: 0 };
        experience = experience.Value;
        const experienceScoreMultiple = this.getTitleData( CONSTS.TITLE_DATA_KEYS.experienceScoreMultiple );
        return experience + Math.floor( score * experienceScoreMultiple );
    },

    determineLevel: function( exp ){
        const growthConstant = this.getTitleData( CONSTS.TITLE_DATA_KEYS.experienceGrowthConstant );
        const level = Math.ceil( growthConstant * Math.sqrt( exp ) );
        return level;
    },

    initializePlayerStatistics: function() {
        this.updatePlayerScore( 0 );
    },

    calculateMayhem: function( score ) {
        return Math.floor( score * this.getTitleData( CONSTS.TITLE_DATA_KEYS.mayhemMultiplier ) );
    },

    calculateGluttony: function( score ) {
        return Math.floor( score * this.getTitleData( CONSTS.TITLE_DATA_KEYS.gluttonyMultiplier ) );
    },

    updatePlayerScore: function( score, bestCombo = 0, isEvent = false ) {
        const stats = this.getPlayerStats();
        const experience = this.createExperience( stats, score );
        const level = this.determineLevel( experience );
        let scoreStatistic, currencyAddition;
        // adjust different statistics depending on event
        if( !isEvent ) {
            scoreStatistic = {
                StatisticName: CONSTS.STATISTIC_NAMES.score,
                Value: score
            };
            currencyAddition = {
                VirtualCurrency: CONSTS.CURRENCY_NAMES.mayhem,
                Amount: this.calculateMayhem( score )
            };
        } else {
            scoreStatistic = {
                StatisticName: CONSTS.STATISTIC_NAMES.dailyChallenge,
                Value: score
            };
            currencyAddition = {
                VirtualCurrency: CONSTS.CURRENCY_NAMES.gluttony,
                Amount: this.calculateGluttony( score )
            };
        };
        // update statistics
        server.UpdatePlayerStatistics( helpers.createRequest({
            Statistics: [
                scoreStatistic,
                {
                    StatisticName: CONSTS.STATISTIC_NAMES.experience,
                    Value: experience
                },
                {
                    StatisticName: CONSTS.STATISTIC_NAMES.level,
                    Value: level
                },
                {
                    StatisticName: CONSTS.STATISTIC_NAMES.bestCombo,
                    value: bestCombo
                }
            ]
        }));
        // update currencies
        server.AddUserVirtualCurrency(
            helpers.createRequest( currencyAddition )
        );
        return [ level, stats ];
    },

    calculateConsecutives( stats, isVictory ) {
        // update consecutive wins/Losses
        let consecutiveLosses = this.extractNumericStatistic( stats, CONSTS.STATISTIC_NAMES.consecutiveLosses );
        let consecutiveWins = this.extractNumericStatistic( stats, CONSTS.STATISTIC_NAMES.consecutiveWins );
        if( isVictory ) {
            consecutiveWins += 1;
            consecutiveLosses = 0;
        } else {
            consecutiveLosses += 1;
            consecutiveWins = 0;
        }
        // update win stats
        this.updatePlayerWinStatistics( consecutiveWins, consecutiveLosses );
        stats[ CONSTS.STATISTIC_NAMES.consecutiveWins ] = consecutiveWins;
        stats[ CONSTS.STATISTIC_NAMES.consecutiveLosses ] = consecutiveLosses;
        return stats;
    },

    calculateRewards: function( stats, newLevel ) {
        const rewards = [];
        const consecutiveLosses = stats[ CONSTS.STATISTIC_NAMES.consecutiveLosses ];
        const oldLevel = this.extractNumericStatistic( stats, CONSTS.STATISTIC_NAMES.level );
        const titleData = this.getTitleData( [
            CONSTS.TITLE_DATA_KEYS.failuresBeforeRewards,
            CONSTS.TITLE_DATA_KEYS.failureRewards,
            CONSTS.TITLE_DATA_KEYS.levelUpRewards
        ] );
        const nFailures = titleData[ CONSTS.TITLE_DATA_KEYS.failuresBeforeRewards ];
        const failureRewards = JSON.parse( titleData[ CONSTS.TITLE_DATA_KEYS.failureRewards ] );
        const levelUpRewards = JSON.parse( titleData[ CONSTS.TITLE_DATA_KEYS.levelUpRewards ] );
        // check for failure reward
        if( consecutiveLosses > 0 && consecutiveLosses % nFailures === 0 ) {
            rewards.push( failureRewards );
        }
        // check for levlUp reward
        if( oldLevel < newLevel ) {
            let nLevel = oldLevel;
            while( ++nLevel <= newLevel ) {
                // grant nLevel award
                if( levelUpRewards[ nLevel ] ) {
                    rewards.push( levelUpRewards[ nLevel ] );
                } else {
                    // grant highest nLevel reward
                    const max = Object.keys( levelUpRewards ).sort( (a, b) => {
                        return parseInt( a ) - parseInt( b );
                    }).pop();
                    if( nLevel >= max ) {
                        rewards.push( levelUpRewards[ max ] );
                    }
                }
            }
        }
        if( rewards.length ) {
            return this.grantRewards( rewards );
        }
        return [];
    },

    extractNumericStatistic: function( stats, key ) {
      return stats[ key ] ? stats[ key ].Value : 0;
    },

    updatePlayerWinStatistics: function( wins, losses ) {
      // update statistics
      server.UpdatePlayerStatistics( helpers.createRequest({
          Statistics: [
              {
                  StatisticName: CONSTS.STATISTIC_NAMES.consecutiveWins,
                  Value: wins
              },
              {
                  StatisticName: CONSTS.STATISTIC_NAMES.consecutiveLosses,
                  Value: losses
              }
          ]
      }));
    },

    grantRewards: function( rewards ) {
        const request = this.createRequest();
        const currencyRequests = [];
        const itemRequest = {
            Annotation: "Level Up Reward",
            ItemIds: []
        };
        const returnRewards = [];
        for( let reward of rewards ) {
            switch( reward.type.toLowerCase() ) {
                case 'currency':
                    let index = currencyRequests.findIndex( request => {
                        return request.VirtualCurrency === reward.key
                    });
                    if( index >= 0 ) {
                        currencyRequests[ index ].Amount += parseInt( reward.quantity );
                    } else {
                        currencyRequests.push({
                            VirtualCurrency: reward.key,
                            Amount: parseInt( reward.quantity )
                        });
                    }
                    break;
                case 'container':
                    // fallthrough
                case 'item':
                    for( let i = 0; i < reward.quantity; i++ ) {
                        itemRequest.ItemIds.push( reward.key );
                    }
                    break;
                default:
                    break;
            }
        }
        if( itemRequest.ItemIds.length ) {
            let items = server.GrantItemsToUser( Object.assign( itemRequest, request ) );
            if( items && items.ItemGrantResults ) {
                returnRewards.push( ...items.ItemGrantResults.filter( grant => { return grant.Result })
                    .map( grant => {
                         return {
                            Amount: grant.UsesIncrementedBy,
                            Remaining: grant.RemainingUses,
                            InstanceId: grant.ItemInstanceId,
                            Id: grant.ItemId,
                            Name: grant.DisplayName,
                            Type: grant.ItemClass.toLowerCase()
                         };
                     }
                ));
            }
        }
        // log.debug( 'item_result', items );
        for( let currencyRequest of currencyRequests ) {
            let currencies = server.AddUserVirtualCurrency( Object.assign( currencyRequest, request ) );
            if( currencies ) {
                returnRewards.push({
                   Amount: currencies.BalanceChange,
                   Balance: currencies.Balance,
                   Id: currencies.VirtualCurrency,
                   Type: 'currency'
                });
            }
            // log.debug( 'currency_result', currencies );
        }
        return returnRewards;
    },

    updatePlayerBugsEaten: function( bugs ) {
        const request = helpers.createRequest({
            Statistics: [{
                StatisticName: CONSTS.STATISTIC_NAMES.bugs,
                Value: bugs
            }]
        });
        server.UpdatePlayerStatistics( request );
    },

    determineLevelScoreRequirement: function( level ) {
        const scoreMultiple = this.getTitleData( CONSTS.TITLE_DATA_KEYS.experienceScoreMultiple );
        const growthConstant = this.getTitleData( CONSTS.TITLE_DATA_KEYS.experienceGrowthConstant );
        return Math.ceil( ( Math.pow( ( level - 1 ) / growthConstant, 2 ) / scoreMultiple ) );
    },

    determineLevelExpRequirement: function( level ) {
        const growthConstant = this.getTitleData( CONSTS.TITLE_DATA_KEYS.experienceGrowthConstant );
        return Math.ceil( Math.pow( ( level - 1 ) / growthConstant, 2 ) );
    },

    getPlayerStats: function() {
        let request = this.createRequest({
            "StatisticNames": [
                CONSTS.STATISTIC_NAMES.score,
                CONSTS.STATISTIC_NAMES.experience,
                CONSTS.STATISTIC_NAMES.level,
                CONSTS.STATISTIC_NAMES.dailyChallenge,
                CONSTS.STATISTIC_NAMES.bugs,
                CONSTS.STATISTIC_NAMES.bestCombo,
                CONSTS.STATISTIC_NAMES.consecutiveLosses,
                CONSTS.STATISTIC_NAMES.consecutiveWins
            ]
        });
        let statisticReturn = server.GetPlayerStatistics( request );
        const statistics = statisticReturn.Statistics;

        const returnStats = {};
        for( let s in statistics ) {
            const stat = statistics[ s ];
            returnStats[ stat.StatisticName ] = stat;
        }
        returnStats.currencies = this.getPlayerCurrencies();
        return returnStats;
    },

    getPlayerCurrencies: function() {
        request = this.createRequest();
        let inventoryReturn = server.GetUserInventory( request );
        return inventoryReturn.VirtualCurrency;
    },

    getInputError: function( input, keys ) {
        if( !input ) {
            return `no arguments`;
        }
        for( let key of keys ) {
            if( !input[ key ] ) {
                return `${key} is required`;
            }
        }
        return false;
    },

    generateDailyChallengeSeed: function() {
        const offset = 100000000;
        const difference = 999999999 - offset;
        const random = Math.floor( Math.random() * difference );
        return random + offset;
    },

    getDailyChallengeRegenTime: function() {
        const lastChangeTimeResponse = server.GetTitleInternalData({
            Keys: [ CONSTS.INTERNAL_DATA_KEYS.challengeLastRegenTime ]
        });
        let lastChangeTimeResponseData;
        if( lastChangeTimeResponse.Data ) {
            lastChangeTimeResponseData = lastChangeTimeResponse.Data[ CONSTS.INTERNAL_DATA_KEYS.challengeLastRegenTime ];
        }
        return parseInt( lastChangeTimeResponseData ) || 0;
    },

    getDailyChallengeSeed: function() {
        const levelChangeTime = this.getLevelChangeTime().getTime();
        const dailyChallengeRegenTime = this.getDailyChallengeRegenTime();
        // type insensitive match
        if( dailyChallengeRegenTime !== levelChangeTime ) {
            const seed = this.generateDailyChallengeSeed();
            server.SetTitleInternalData({
                "Key": CONSTS.INTERNAL_DATA_KEYS.challengeLastRegenTime,
                "Value": levelChangeTime
            });
            server.SetTitleInternalData({
                "Key": CONSTS.INTERNAL_DATA_KEYS.challengeSeed,
                "Value": seed
            });
            // do not return result
            return -1;
        } else {
            const response = server.GetTitleInternalData({
                keys: [ CONSTS.INTERNAL_DATA_KEYS.challengeSeed ]
            });
            return response.Data ? response.Data[ CONSTS.INTERNAL_DATA_KEYS.challengeSeed ] : -1;
        }
    },

    getTitleData: function( key_s ) {
        if( typeof key_s === "string" ) {
            const response = server.GetTitleData({ keys: [ key_s ] });
            return response.Data ? response.Data[ key_s ] : -1;
        } else {
            const response = server.GetTitleData({ keys: key_s });
            return response.Data ? response.Data : null;
        }
    },

    dayToIndex: function( dayStr ) {
        const days = [ "sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday" ];
        return days.indexOf( dayStr.toLowerCase() );
    },

    isBetweenDaysInclusive: function( check, start = 0, end = 0 ){
        for( let i of [ check, start, end ] ) {
            if( i < 0 || i > 6 ) {
                return false;
            }
        }
        let i = start;
        while( i !== end ) {
            if( i === check ) {
                return true;
            }
            i = ++i % 7;
        }
        return i === check;
    },

    isDailyChallenge: function( offset = 0 ) {

        offset = parseInt( offset );
        if( isNaN( offset ) ) {
            offset = 0;
        }

        const challengeLimits = server.GetTitleInternalData({
            keys:[ CONSTS.INTERNAL_DATA_KEYS.challengeStart, CONSTS.INTERNAL_DATA_KEYS.challengeEnd ]
        })

        const utcDate = new Date( new Date().toUTCString() );
        const utcMillis = utcDate.getTime();
        const offsetMillis = utcMillis + ( offset * 3600000 );
        const now = new Date( offsetMillis );
        const currentDay = now.getDay();
        const startIndex = this.dayToIndex( challengeLimits.Data[ CONSTS.INTERNAL_DATA_KEYS.challengeStart ] );
        const endIndex = this.dayToIndex( challengeLimits.Data[ CONSTS.INTERNAL_DATA_KEYS.challengeEnd ] );

        return this.isBetweenDaysInclusive( currentDay, startIndex, endIndex );

    },

    getUserFirstLoginTime: function() {
        const accountData = server.GetUserAccountInfo( this.createRequest() );
        if( accountData.UserInfo ) {
            return accountData.UserInfo.Created;
        }
        return 0;
    },

    getLevelChangeTime: function() {
        var d = new Date( Date.now() );
        var now = new Date( Date.now() );
        d.setHours( this.getTitleData( CONSTS.TITLE_DATA_KEYS.challengeLevelRegenHour ), 0, 0, 0 );
        if( d > now ) {
            d.setDate( d.getDate() - 1 );
        }
        return d;
    },

    removeAnimosity: function( animosity ) {
        const currencies = this.getPlayerCurrencies();
        const playerAnimosity = currencies[ CONSTS.CURRENCY_NAMES.animosity ];
        if( playerAnimosity && playerAnimosity >= animosity ) {
            server.SubtractUserVirtualCurrency( this.createRequest({
                VirtualCurrency: CONSTS.CURRENCY_NAMES.animosity,
                Amount: animosity
            }));
            return true;
        }
        return false;
    }

}
