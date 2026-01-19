# FixMeta

## What does this app do?
FixMeta is a simple CLI that sets the "PreventDashLaunch" registry key to fix the current issues with Meta revoking users' access to the Dash app

## I don't want to give admin access!
That's fine! Below is a simple step-by-step guide on how to set the key manually.
1) Launch the `Registry Editor` app
2) Either click through the options until you find `Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Oculus VR, LLC\Oculus\Config` or paste into the bar at the top
3) Right click in the empty space on the right side and create a new DWord
4) Put `PreventDashLaunch` as the key name
5) Double click on the new key you created and set it to `1` to disable dash or `0` to enable dash
6) That's it!

### Notes:
This app requires Administrator access to work! Setting registry keys is an administrative action on Windows and will not function without!
