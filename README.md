# FreyrEssentials
*Tools I wouldn't function without* 💻


## Timer class
Create a countdown and start the timer
``` c#
Timer myTimer = Timer.Create(5f);
```
read the value as a normal progression of the countdown ranging from 0 to 1
``` c#
Vector2.Lerp(pointA, pointB, myTimer)
```
or as a bool
``` c#
if(myTimer)
```

## Pool\<T> class  
_Lightweight generic pooling class._

### Why use Pool?
Pool is just a simple lightweight pooling system which takes care of the pooling boilerplate.

### How to use
<img src="https://static.wikia.nocookie.net/dragonquest/images/6/60/Slime_Artwork.png/revision/latest/scale-to-width-down/1000?cb=20201021141416" alt="drawing" width="80"/><img/>

#### Constructing a pool
``` c#
Pool<Slime> slimePool = new Pool<Slime>
            (() => new GameObject().AddComponent<Slime>(),
            (isSpawning, slime) => slime.gameObject.SetActive(isSpawning));
```
The first parameter called `CreateNew` is a `Func<T>` which will be used
by the Pool class to create new instances of `T`.
So what I'm doing above is instantiating a new GameObject and adding to it a _Slime_ component which more importantly returns the instance.
``` c#
() => Instantiate(gameObject).AddComponent<Slime>() 
```
The second parameter is optional, called `OnChange` which is an `Action<bool, T>` and runs when a T instance is being either __borrowed__ or __returned__.
What I do is use it to enable/disable the _Slime's_ gameObject depending on that factor.
``` c#
(isSpawning, slime) => slime.gameObject.SetActive(isSpawning)
```
#### Borrow/Return
Now that you've made a pool you can start borrowing and returning some instances. Use `slimePool.Borrow()` and `slimePool.Return()` respectively. That's all.
