# Freyr_C_Sharp_Essentials
*Tools I wouldn't function without*


## Timer class
Create a countdown and start the timer
``` c#
Timer myTimer = Timer.Create(5f);
```
read the value as a normal progression of the countdown _(0 to 1)_
``` c#
Vector2.Lerp(pointA, pointB, myTimer)
```
or a bool _(is finished?)_
``` c#
if(myTimer)
```
