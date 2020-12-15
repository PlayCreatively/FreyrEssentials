# Timer class
Create a countdown and start the timer
``` c#
//Create and start a count down of 5 seconds.
Timer myTimer = Timer.Create(5f);
```
read the value as a normal progression of the countdown ranging from 0 to 1
``` c#
//Interpolate between pointA and pointB by myTimer's progression to 5 seconds.
Vector2.Lerp(pointA, pointB, myTimer)
```
or as a bool
``` c#
//While myTimer hasn't reached 5 seconds.
while(!myTimer)
```

___

# Pool\<T> class  
_Lightweight generic pooling class._

### Why use Pool?
Pool is just a simple lightweight pooling system which takes care of the pooling boilerplate.

### How to use <img src="https://static.wikia.nocookie.net/dragonquest/images/6/60/Slime_Artwork.png/revision/latest/scale-to-width-down/1000?cb=20201021141416" draggable="false" alt="drawing" width="30"/><img/>

#### Constructing a pool
``` c#
Pool<Slime> slimePool = new Pool<Slime>
            (() => new GameObject().AddComponent<Slime>(),
            (isSpawning, slime) => slime.gameObject.SetActive(isSpawning));
```
The first parameter called `CreateNew` is a `Func<T>` which will be used
by the Pool class to create new instances of `T`.
``` c#
//Method which instantiates a gameObject, adds a slime component and returns a reference to it.
() => new GameObject().AddComponent<Slime>() 
```
The second parameter is optional, called `OnChange` which is an `Action<bool, T>` and runs when a T instance is being either __borrowed__ or __returned__.
``` c#
//Enable/disable the slime component based on if it's being borrowed or returned.
(isSpawning, slime) => slime.gameObject.SetActive(isSpawning)
```
#### Borrow/Return
Now that you've made a pool you can start borrowing and returning slime instances using `slimePool.Borrow()` and `slimePool.Return(Slime)` respectively.
