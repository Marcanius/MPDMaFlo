# MPDMaFlo
De repository for Multi Platform assignment.

Dance of the Gardeners is a turn-based strategy game where players take turns placing features in a garden.

The game was built around an input system which allowed the game to be built for android phones, as well as windows.

An overview of the most important classes:

Extensions - 
	This static class holds various variables and methods that are used by multiple classes, to keep those classes loosely coupled from eachother.

Input Manager - 
	This static provides the game with the correct input method, depending on the platform for which the game was built.

State Manager - 
	This static class holds various variables and methods that are used in the game to represent and change the game state. 
	It holds information on whose and which turn it is, and has methods to change these things.
