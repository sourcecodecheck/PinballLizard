#define APPCENTER_UNITY_USE_CRASHES
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "AppCenterStarter.h"
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@import AppCenter;

#ifdef APPCENTER_UNITY_USE_CRASHES
@import AppCenterCrashes;
#endif

#ifdef APPCENTER_UNITY_USE_PUSH
@import AppCenterPush;
#import "../Push/PushDelegate.h"
#endif

#ifdef APPCENTER_UNITY_USE_ANALYTICS
@import AppCenterAnalytics;
#endif

#ifdef APPCENTER_UNITY_USE_DISTRIBUTE
@import AppCenterDistribute;
#import "../Distribute/DistributeDelegate.h"
#endif

enum StartupMode {
  APPCENTER,
  ONECOLLECTOR,
  BOTH,
  NONE,
  SKIP
};

@implementation AppCenterStarter

static NSString *const kMSAppSecret = @"70441c51-3099-49b3-90b9-2a84f8a277fa";
static NSString *const kMSTargetToken = @"appcenter-transmission-target-token";
static NSString *const kMSCustomLogUrl = @"custom-log-url";
static NSString *const kMSCustomApiUrl = @"custom-api-url";
static NSString *const kMSCustomInstallUrl = @"custom-install-url";

static const int kMSLogLevel = 4;
static const int kMSStartupType = 0;

+ (void)load {
  [[NSNotificationCenter defaultCenter] addObserver:self
                                           selector:@selector(startAppCenter)
                                               name:UIApplicationDidFinishLaunchingNotification
                                             object:nil];
}

+ (void)startAppCenter {
  NSInteger startTarget = kMSStartupType;
  if (startTarget == SKIP) {
    return;
  }

  NSMutableArray<Class>* classes = [[NSMutableArray alloc] init];

#ifdef APPCENTER_USE_CUSTOM_MAX_STORAGE_SIZE
  [MSAppCenter setMaxStorageSize:APPCENTER_MAX_STORAGE_SIZE completionHandler:^void(bool result){
    if (!result)
      MSLogWarning(@"MSAppCenter", @"setMaxStorageSize failed");
  }];
#endif

#ifdef APPCENTER_UNITY_USE_ANALYTICS
  [classes addObject:MSAnalytics.class];
#endif

#ifdef APPCENTER_UNITY_USE_PUSH
    [MSPush setDelegate:[UnityPushDelegate sharedInstance]];
#endif

#ifdef APPCENTER_UNITY_USE_DISTRIBUTE
  
#ifdef APPCENTER_UNITY_USE_CUSTOM_API_URL
  [MSDistribute setApiUrl:kMSCustomApiUrl];
#endif // APPCENTER_UNITY_USE_CUSTOM_API_URL

#ifdef APPCENTER_UNITY_USE_CUSTOM_INSTALL_URL
  [MSDistribute setInstallUrl:kMSCustomInstallUrl];
#endif // APPCENTER_UNITY_USE_CUSTOM_INSTALL_URL
  [classes addObject:MSDistribute.class];

#endif // APPCENTER_UNITY_USE_DISTRIBUTE

  [MSAppCenter setLogLevel:(MSLogLevel)kMSLogLevel];

#ifdef APPCENTER_UNITY_USE_CUSTOM_LOG_URL
  [MSAppCenter setLogUrl:kMSCustomLogUrl];
#endif
  switch (startTarget) {
    case APPCENTER:
      [MSAppCenter start:kMSAppSecret withServices:classes];
      break;
    case ONECOLLECTOR:
      [MSAppCenter start:[NSString stringWithFormat:@"target=%@", kMSTargetToken]
            withServices:classes];
      break;
    case BOTH:
      [MSAppCenter start:[NSString stringWithFormat:@"appsecret=%@;target=%@", kMSAppSecret, kMSTargetToken]
            withServices:classes];
      break;
    case NONE:
      [MSAppCenter startWithServices:classes];
      break;
  }
}

@end
