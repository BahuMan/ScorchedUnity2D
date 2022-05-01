# ScorchedUnity2D
my own re-interpretation of the mother of all games
The DOS game is still downloadable at: http://www.whicken.com/scorch/

## Current status

* [ ] More weapons!   *More!*   ***MORE!***
  * [x] Baby Missile
  * [x] MIRV
  * [x] Funky Bomb!
  * [x] Dirt Bomb
* [ ] AI
  * [x] Moron (shoots completely random)
  * [x] Shooter (always shoots at max power
  * [ ] Tosser (needs rework)
  * [ ] Spoiler (Calculates ballistic trajectory, but doesn't take into account wind or viscosity)
* [x] select active weapon from current inventory
* [x] buy menu between rounds
  * [ ] AI doesn't buy yet
* [x] at end of round, move to next round (new level, new terrain)
* [x] main menu
  * [x] sets nr of players
  * [x] sets nr of rounds
  * [ ] other preferences, like border style, starting money, ...
* [x] Add human/AI players and provide name for human players
* [x] naive, big blocks of destructible terrain
  * [x] home-brewn quadtree colliders will split for explosions and merge for dirt bombs
  * [ ] suspended dirt does not (yet) fall
* [X] wind
* [x] user controls tank
* [x] explosion destroys terrain (Dirt bomb adds terrain)
* [ ] destroyed tank animation
* [x] GUI reflects current tank's status
  * [x] angle, power and HP are updated real-time; 
* [x] In-game escape menu
  * [x] physics time is stopped when calling up menu
  * [x] resume, quit, fast-forward work
  * [x] kill all and move to next round
  * [ ] change border, wind options ?
* [x] screen borders
  * [x] solid
  * [x] bounce
  * [x] wrap
* [x] use talk files to say something before shooting
* [ ] use talk files to say something while dying
* [ ] use destructible terrain from <https://github.com/Ideefixze/DTerrain>
* [ ] Network multiplayer

![screenshot](info/Screenshot-mainmenu.jpg?raw=true)
![screenshot](info/Screenshot-addplayer.jpg?raw=true)
![screenshot](info/Screenshot-2022-04-18.jpg?raw=true)
![screenshot](info/Screenshot_menu.jpg?raw=true)
