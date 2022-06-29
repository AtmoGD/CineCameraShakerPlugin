# CineShaker

A simple but usefull camera shake plugin for cinemachine.

---

## Installation

Install the package with the git link. If you haven't installed cinemachine yet, it will be automatically.

> NOTE: If you don't know how to import git packagages [here](https://docs.unity3d.com/Manual/upm-ui-giturl.html) you can find how to do it.

---

## Setup
To use CineShaker you have to place a CineShakerComponent in your scene. The component needs a reference to two thigs to work:
1. A cinemachine brain. If you have just one camera you can leave this field empty because the component will fetch it by its own.
2. A default `ShakeOptions` objekt.

---

## Create ShakeOptions
ShakeOptions are files which contains data about the shake effekt. Here you can set the `Duration`, `Amplitude` and `Frequency` of the effect. You can create as much objects as you want (For example: One for light shakes and one for heavy shakes).

To create a ShakeOption Object simply right click in the project view -> `Create` -> `CineShaker` -> `New shake settings`

Now you are ready to use CineShaker!

---

## Usage
To let the camera move you now just have to call the `Shake()` function of the CineShakerComponent. Since CineShaker is a Singleton you can call everywhere `CineShaker.Instance.Shake()`. This will use the default ShakeOptions. To use different options you can use the `Shake(ShakeOptions _options)` function. To change the `defaultShakeOptions` at runtime you can call the `SetNewDefaultOptions(ShakeOptions _options)`function.

