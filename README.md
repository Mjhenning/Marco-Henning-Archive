# Marco Henning Archive

## Description
An archive setup to showcase various projects developed and completed during Marco Henning's student courses, the goal of this archive is to show my understanding of various elements of game design/ development and to showcase how I adapt and use new things I've learned.

Here's a list of each and what they were made for:
- Tiny Tibe Generation Scene (Group Student Project)
- Desktop Pet Prototype (Student Project)
- Networked Prorotype (Student Project)
- Open World Prototype (Student Project)


## Project Intentions

### Tiny Tribe Generation Scene
Originally developed as part of a larger group project where we had to develop a functioning tiny tribe comprised of independent interacting systems. Each system was developed by individual students. My system was specifically the generation system of the environment and resources; this system was later also expanded to help spawn animals, civilian AI and starting villages.
### Desktop Pet Prototype
A personal student project developed over the span of a semester. We were tasked with bringing to life a virtual AI pet that convincingly simulates intelligence and complex behaviour. The essence of this challenge lied in designing an AI that not only reacts in a nuanced manner to user interactions but also incorporates external factors to provide dynamic and engaging feedback. This pet was also at the end of the project tasked to be compatible with Android devices.
### Networked Prototype
A personal student project developed over the span of a semester. We were tasked with creating a networked multiplayer arcade game prototype that demonstrates the understanding of peer-to-peer networking in Unity using Mirror during the first 8 weeks. During the final 8 weeks we took this arcade prototype and turned it into a team-based prototype that can be deployed on a dedicated server infrastructure on AWS. This prototype had to have a continuous gameplay loop, allowing players to join, engage in the current round, and continue playing until they're ready to leave.
### Open World Prototype
A personal student project developed over the span of a term. We were asked to develop a sandbox game with modular, interacting systems. The main focus of this assignment was to gain experience with data management, inter-system communication, and project architecture. We were also limited to developing the game on a small island in order to limit the scope and to provide restrictions on these systems.


## Noteworthy integration in Projects

### Tiny Tribe Generation Scene
Fully procedrual generation system consisting of multiple generators working together to generate one full environment at once:
- Terrain generator, generates full terrain with a mesh and colors to represent each "level" in the Terrain
- Town Generator, choses random spots on the terrain to generate a bubble where within a town generates itself with a town hall, a specified randomized amount of civilian huts and resource storage.
- Resource Generator, same approach as the town generator but only has one bubble with a randomized amount of resources within a specified heigh range.
- Animal Spawner, derived from the resource generator and works in a very similar vein.

Also added some scripts that can be used by other systems to acess data from the generated environment.

### Desktop Pet Prototype
Main noteworthy things from this project is a procedurally generated shop, items in said shop and randomized responses and actions from the vendor.
- The shop generator is randomized based off of parameters and a background sanity system that influenced by player actions such as discarding shop items or ignoring the vendor.
- The items inside the shop is randomized based off of various user defined prefixes, suffixes, descriptions and even influenced by the specific set / type combination of each item. Each item sprite is also defined by it's specfic set/type combination.
- The vendor's actions are defined by a state tree with user defined parameters that are slightly influenced by the sanity system. API integrations are used to help make responses more interresting. Either a fact or death counter API is used based on how "corrupt" the vendor has become, generic user defined responses are distorted and changed based on the vendor's sanity.
- Various smaller interactions were coded for the vendor, such as changing his mask, waking him up when he's asleep and even changing the vendor's tie's colour.

### Networked Prototype
Main impressive feat is having a functional networked prototype with a continous loop and alot of visual feedback and colors used to identified players. 
Global scoreboards are also implemented that sync up between all isntances of the game. 
One small script was added for convenience when it comes to launching, restarting or stopping the server on a linux machine.

### Open World Prototype
Impressive feats include functioning inventory and crafting system and item states that are influenced by envrionmental interactions.
- Serailized inventory system that can be clicked and dragged around to change the slots items are in or to swap the slots between items. A functional hotbar is also coded that can easily be customized in the inventory.
- Items are influenced by a fire placed in the envrionment, the stats on these items change based on how cooked they are and their sprites and uses also change.
- A very basic crafting system with a singular recipe was also created.