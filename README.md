# Swarm Tutorial (built on top of SpatialOS)
---



*GitHub Repository*: [https://github.com/ananda13/swarm](https://github.com/ananda13/swarm)

---

## Introduction

This is a tutorial on how to setup a swarm simulation using [spatialOS](https://spatialos.improbable.io/), which is created by Improbable.

Disclaimer: I am not a spatialOS expert, but a simulation enthusiast, and the intent of the tutorial is to understand how to build large scale simulations with SpatialOS, with the bulk of the computation (physics) running on the cloud. The additional advantage is that you can have multiple users log into it, to view or interact with the simulation

In this tutorial, we will set up a VR enabled (HTC Vive) simulation of a swarm.


## Contents

### Part I   : Basics & Boids
### Part II  : Plug Into Improbable
### PART III : Get Into VR

<br />
<br />

## PART I

#### Getting Started

Skills required: 

a. Basic Unity/C#

b. SpatialOS

You will need to familiarize yourself with the basics of SpatialOS, which is a key component of this tutorial.
They have excellent starter tutorials [here](https://spatialos.improbable.io/docs/reference/10.1/tutorials/showcase)

If you have a basic grasp of Unity, and know nothing about SpatialOS, I would suggest jumping in and trying it. I'm not a Unity expert, and it took me a few days to build this; and I'm sure you can too!

<br />
<br />

*SWARMS*

So what is a swarm, and why simulate it?

Swarms are really interesting because at a micro level, they follows extremely simple rules, but at a macro level, the effects look very sophisticated and frankly, awe-inspiring!

Just look at these guys:

![Starlings Murmurations](/images/starling-murmurations.gif)

###### Above: A 'murmuration' of starlings . A poet probably came up with that name. 
###### Murmurations are a more complex subset of flocking or swarming behavior.


<br />
<br />

*BOIDS*
<br />

#### Theory 

Craig Reynold's created the first flocking algorithm, and called the simulated creatures 'boids'. You can read all about it [here](http://www.red3d.com/cwr/boids/)

I've summarized the basic principles below. 

If you have a group of creatures, you need them to form a group, with these 3 basic rules in place:

1. Cohesion

Move each boid towards the average position of the group

2. Alignment

Align the heading of each boid towards the average heading of the group

3. Separation

Steer away from other boids to prevent collision.<br />



I've added a fourth rule, so that the flock has a goal to follow:

4. Goal Seeking

Steer each boid towards an arbitrary goal<br />


You can also add more sophisticated rules such as collision avoidance, goal intelligence, etc<br />
<br />


#### Code

There are many in-depth tutorials on how to build a swarm online. I built the boid behavior based on (this one)[https://www.youtube.com/watch?v=eMpI1eCsIyM&t=913s].


I've built a simple flocking example in Unity. You can check out this code in [this repo] (https://github.com/ananda13/basic-swarm). You dont, strictly speaking, need to build this in order to proceed, but it does help you understand flocking behavior before you transition to the Improbable way of doing things!

The important section of the codebase are :

A. globalflock.cs
This defines the behavior of the goal, and sets up the global variables, such as number of boids, etc<br />


B. flock.cs
Provides all the rules that each entity follows. This is the embodiment of the 4 rules above.<br />



