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
### Basics & Boids

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

There are many in-depth tutorials on how to build a swarm online. I built the boid behavior based on [this one](https://www.youtube.com/watch?v=eMpI1eCsIyM&t=913s).


I've built a simple flocking example in Unity. You can check out this code in [this repo](https://github.com/ananda13/basic-swarm). You dont, strictly speaking, need to build this in order to proceed, but it does help you understand flocking behavior before you transition to the Improbable way of doing things!

The important section of the codebase are :

A. *globalflock.cs* <br />

This defines the behavior of the goal, and sets up the global variables, such as number of boids, etc<br />


B. flock.cs <br />

Provides all the rules that each entity follows. This is the embodiment of the 4 rules above.<br />

<br />
<br />
<br />
## PART II
### Plug Into Improbable

At this point, if you don't know what Improbable or SpatialOS is, I would highly encourage you to run through their excellent [tutorials](https://spatialos.improbable.io/docs/reference/10.1/tutorials/showcase).

1. [Sign up for their Beta](https://spatialos.improbable.io/get-spatialos)<br />

2. [Setup your machine to run spatialOS](https://spatialos.improbable.io/docs/reference/10.1/getting-started/requirements)<br />

3. [Helloworld tutorial](https://spatialos.improbable.io/docs/reference/10.1/tutorials/helloworld/hello-world) : Get your feet wet by building, running locally and deploying to the cloud.<br />

4. [PiratesTutorial](https://spatialos.improbable.io/docs/reference/10.1/tutorials/pirates/overview) : Understand the basic mechanism of client-server architecture. This is the most important tutorial, and the rest of this tutorial is based off this one.
<br />

Once you're past these (and I would love your feedback on what you thought of these), the following may help you setup a simulation model, as opposed to a game. (We will gamify and add VR to this sim later!)
<br />

As you have noticed, the client-server architecture underpins any spatialOS setup. SpatialOS will let you create UnityServers and UnityClients on which you can run separate pieces of code. In general if you need a window into the world, or need to control anything within this world, you'll need a client, otherwise, everything runs on the server side. The advantage of using SpatialOS, is that now you can run hundreds of thousands of entities in a swarm, and spatialOS will manage all the underlying complexity, and make it work like magic. 

Reality scale simulation. That's pretty cool!

Now, onto the fun stuff. Lets get a swarm going.


Visually, this is how I've architected the simulation:


<insert image>


The code is on GitHub here. Please check out the [tag v0.1](https://github.com/ananda13/swarm/releases/tag/v0.1), if you want to start with some basics and add on the rest yourself. Otherwise check out the [latest](https://github.com/ananda13/swarm/).

The main files to use are:


BUILD

This is a local build, also known as a development build, in spatialOS.

If you're on Windows, use the Improbable drop down menu within Unity to run these functions, and if you're on Mac, use command line on terminal. In fact, as of the date of writing this, if you dont follow the above convention, you _may_ run into strange compile errors.

Build (Mac):

```
$ spatial worker build --target=development
```

Launch locally:
```
$ spatial local launch --snapshot=snapshots/initial_world.snapshot 	
```

(Windows)
Window > SpatialOS 



In a browser window navigate to:
http://localhost:21000/inspector




DEPLOYMENT

run

Create the deployment assets, specifically for cloud deployment
```
$ spatial worker build --target=deployment
```

Upload to the cloud (GCP)
```
$ spatial cloud upload <assembly_name> 
```
(You can use something like SwarmSimAssembly)

For example:
```
$ spatial cloud launch SwarmSimAssembly default_launch.json swarm --snapshot=snapshots/initial_world.snapshot 
```

Check it out:
Open https://console.improbable.io/projects.
<br />

You should see something like this:<br />

![Inspector View](/images/swarm-improbable-inspector-1.gif)

<br />




## PART III
### Get Into VR

Up until now you have built a simulation from scratch on top of spatialOS, and have watched it mesmerize you with the simple beauty of swarming behavior, a murmuration even.

Now we'll plug in a client, that will let you watch from inside the game engine, and then plug in your HTC Vive headset!

Onward, and upward!


#### A. Setting up the client.

In order to setup a client in unity, we need the following entities, with their components:

- Player Spawner : This allows spatial to manage all incoming connections and assign them gameobjects
	- WorldTransform : to know where it it
	- Spawner : to request gameobjects (camera) be spawned for the connecting client


- Player : The entity that embodies each player
	- WorldTransform : to know where in space it is
	- PlayerLifecycle : to keep track of when this player disconnects, by tracking a 'heartbeat'
	
This allows the PlayerSpawner (which is created on boot) to spawn players, as they connect.


As you can see, I've used the PiratesTutorial as reference.
This being SpatialOS, you can have hundreds of people log into the experience, out of the box! We will start with one, but will build the base that will allow any number of players to join in.
Later you can build any interaction in conjunction with multiplayer, and bring about very complex behaviors.

Note: The structure of component-entities will change somewhat once we introduce VR, but it's good to learn about different architectures.


You can find the new schemas for the components in Swarm/schema/improbable/player/

Next, we setup the EntityTemplates that make up the entities. We create the following EntityTemplates:

	- PlayerEntityTemplate (similar to the PlayerShipEntityTemplate in PiratesTutorial)
	<insert image of the acls from this file>
	We make sure that the WorldTransform and PlayerControls can be accessed by the Unity Client


	- PlayerSpawnerEntityTemplate (identical to the PlayerSpawnerEntityTemplate in PiratesTutorial)
	It's important to attach the Spawner component to this entity.

We also add the following scripts to Assets/Gamelogic/Player/Behaviors:

	- PlayerInputController : This takes inputs from the player (in this case, using the mouse and keyboard WASD to control the motion of the player), and updates the PlayerControls component
	
	- PlayerMovement & CameraRotationController : These together receive the component updates of PlayerControls and move the Player gameobject and associated camera
	
	- PlayerSpawnManager : This is attached to the PlayerSpawner prefab, and contains callbacks that are invoked when a new player requests to join the simulation. This spawns the new player and gives them controls to look around and move.

	- PlayerHeartbeatSender : This sends heartbeats (HBs) ever to often (determined by the playerHeartbeatInterval), which is received by the PlayerEntityLifeCycleManager to determine, which player is unresponsive, and they get kicked out.

	- PlayerEntityLifeCycleManager : This basically checks to see if new HBs are coming in from any player, and if it reaches the threshold of missed heartbeats, it deletes the player.


We also need to modify Assets/Bootstrap.cs, and the modifications are identical to the file in PiratesTutorial. This is a what the Unity Client (any player logging in) uses to connect to the simulation. This script first finds the PlayerSpawner, and then requests PlayerSpawner to create a new Player and inject them into the simulation at the designated coordinates (defaulted to (0,0,0))

At this point you can build, and run the simulation locally. Once that is successful, open the project in Unity and play the CLientScene. You should see something like this:
<br />

![Client View](/images/Unity-Client-first-view.png)

Thereafter, build for deployment, create assembly and launch into the cloud, and use the Improbable Launcher to log into client view.
<br />
<br />



#### B. Adding SteamVR







<br />

<br />

<br />

<br />

## Stay In Touch

Follow Ando on [Twitter](https://twitter.com/andoshah)<br />

Follow changes on [GitHub](https://github.com/ananda13/swarm)<br />

Follow the [Improbable Forum](https://forums.improbable.io/)<br />


Let me know if you've made a simulation with Spatial! I'd love to see it!<br />
<br />



## Future

I'm taking up the swarm project, and building a multi-user art-science experiment where players will be able to _try_ to control swarms of different creatures (from starlings to drones!) on islands floating in the sky. In VR (duh)!
<br />

If you're interested in contributing, either with code, art or ideas, feel free to drop me a line.
<br />

Love & Cheers,

Ando
