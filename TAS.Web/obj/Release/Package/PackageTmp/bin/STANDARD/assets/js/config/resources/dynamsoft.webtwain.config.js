//
// Dynamsoft JavaScript Library for Basic Initiation of Dynamic Web TWAIN
// More info on DWT: http://www.dynamsoft.com/Products/WebTWAIN_Overview.aspx
//
// Copyright 2018, Dynamsoft Corporation 
// Author: Dynamsoft Team
// Version: 13.4
//
/// <reference path="dynamsoft.webtwain.initiate.js" />
var Dynamsoft = Dynamsoft || { WebTwainEnv: {} };

Dynamsoft.WebTwainEnv.AutoLoad = true;

///
Dynamsoft.WebTwainEnv.Containers = [{ContainerId:'dwtcontrolContainer', Width:270, Height:350}];

/// If you need to use multiple keys on the same server, you can combine keys and write like this 
/// Dynamsoft.WebTwainEnv.ProductKey = 'key1;key2;key3';
Dynamsoft.WebTwainEnv.ProductKey = 't0068MgAAAHBKrmR5BqPG2EdS1cY1ZtxzP7dGuxbJUvNsWheCDK4X+9bSSzH1j+Xhl/+/29NEfgp14TWW/seQ2yW6BCS2yJI=';
    //'t0068MgAAAHTNGGHg+Vsry7kfAkp5ARs4UkVzFxBNJC12WnyzVMQuOTZqoyqsdJMvYyyQc2DNDHyny7n3IsQl5L/4yfB6bm0=';
///t0068MgAAAHTNGGHg+Vsry7kfAkp5ARs4UkVzFxBNJC12WnyzVMQuOTZqoyqsdJMvYyyQc2DNDHyny7n3IsQl5L/4yfB6bm0=
///580E69B9A2C6DC99B6B86B3BF5A148D109622E245FA69D1F6AF88EA0CAA8B8BCDA29F6491290EF8D142A2002EDBBDD68DA29F6491290EF8D36C3880081D6922109622E245FA69D1FA8CB11D76D462A8340000000;t0068WQAAADoX5VrsfkFllN/gSABeQy2eMNo0T9K5zA5SpKuNC7dczlrKUZhIZTGemlavxPS1xdg8r/EY6Pt2c0dsj5PIDwQ=
Dynamsoft.WebTwainEnv.Trial = true;

///
Dynamsoft.WebTwainEnv.ActiveXInstallWithCAB = false;

///
Dynamsoft.WebTwainEnv.IfUpdateService = false;

///
Dynamsoft.WebTwainEnv.ResourcesPath = 'assets/js/config/resources';

//new settings added by Ranga
Dynamsoft.WebTwainEnv.IfShowUI = false;  
Dynamsoft.WebTwainEnv.IfDuplexEnabled = false;    

/// All callbacks are defined in the dynamsoft.webtwain.install.js file, you can customize them.
// Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', function(){
// 		// webtwain has been inited
// });

