# Animal Anatomy VR

The intent of this game is to allow the student to handle and compare various 
anatomy components of animals. It provides the student with basic VR controls
such as snap turn and teleportation, however, currently the movement is 
limited to just one teleportation spot.

With the structure I implemented, I mapped out a cat with basic structures.
I designed the prefabs and UI to be dynamic and animal agnostic. This allows
the same controls to be used regardless of the subject.

I utilized the XR interaction toolkit to simplfiy the VR components of this
project.

## Project Layout

- `Player` : Standard OpenXR player. This handles the player view, controllers,
turning/teleporting, and interacting.
- `XR Managers` : To keep the project tidy, the managers for UI and XR were 
collected here
- `Room` : The static object that make up the surrounds of the player
	- `Teleportation Points` : Where the player is allowed to teleport
	- `Lights` : The lighting of the enviornment
	- `StaticObjects` : Any object that should not be interacted with (such as the
	room)
- `CatCollection` : Contains the UI and anatomy components of the cat


## Cat Collection

The structure and Components of the objects in this folder define the overall
UI and interaction with the models.

### UI Folder

The `UI` folder provides anchors for the Menu UIs to spawn on. It has
two empties, one for the Body Systems Panel and one for the Systems Parts Panel.

You can see where you are used under `CatCollection/AnatomyCollection->
AnatomyOrganizer`

### AnatomyCollection

This is an empty with my `Anatomy Organizer` script as a component. The role
of this is to collect all of the systems and their parts and display them
on the UI. 

It does this through first reading through all of its children and adding them 
to the UI based on their name. Then, if a System is clicked, it will show it 
and populate the `PartPanel` with the parts in the system. 

Its main task is to keep track of what is showing, show only one piece at a 
time, and to populate the UIs. 

You can mess around with the UI prefab and button prefab if you make nicer ones.

#### Systems

Systems are the main storage of anatomy models. Lets look at the `Skeletal System`
object in the `AnatomyCollection` as an example.

First, it defines what model should be present when the whole system is showing. 
This was to allow the system and the individual components to be at different 
scales for observation.

Next, it has an array of parts prefabs (which can be blank). This populates the 
part panel when the system is selected. 

Lastly, if you want a system showing on start-up, select the `Showing` checkbox.

#### Parts

Parts are composed of three parts: the model, the interaction affordance, and 
the base.

##### Model

This is where you would put in nicer models. Each model should have a `RigidBody`, 
`XR Grab Interactable`, and `Collider` component. 

The `RigidBody` and `XR Grab Interactable` should have the settings specified in the
`InteractableBase` prefab. These settings are also stored as a `Preset` just in case.

The `Collider` should be User added as its shape and type will depend on the model. 
**Keep in mind, this is VR so don't use poly dense meshes or colliders**

##### Interaction Affordance

This just gives the user some color based feedback when they have the model selected.

##### Base

This is the anchor point that the model returns to. After `Wait Time` it will return 
the model over `Lerp Time` seconds. 

## Adding a New Model

1. Create a prefab variant of the `InteractableBase` Prefab in the `Prefabs`
folder.
2. Rename the variant and move it to however you want to organize your anatomy.
3. Drag your model as a child of prefab parent (whatever you named it to)
4. Copy the components from the `Model` present in the Prefab to your new model.
5. Add a collider component that matches your model
6. Delete the old Model component
7. Drag your new model under `Target` in the `Item Recall` component on the parent
8. Drag the new model under `Interactable Source` in the 
`XR Interactable Affordance State Provider` component of the `Interaction Affordance`.
9. Drag the new model under `Renderer` in the `Material Property Block Helper` 
component of the `Color Affordance`.

Congrats, you have added a new model that can be used with the system! For further 
details of how to get it to show on the UI, read below.

## Adding a New Animal

To add a new animal, I would suggest duplicating and renaming `CatCollection`
as a start. (Of which the structure is explained above in [Cat Collection](#cat-collection))

Otherwise:

1. Make an empty to store the collection
2. Create a `UI` empty to define the positions of a `System` and `Part` panel
	- Add an empty for both the `System` and `Part` panels
3. Add an empty with teh `AnatomyOrganizer` script
	- Populate the fields with your UI prefabs and spawn points from `2.`
4. Proceed below with adding your systems...

### Adding Systems

1. Add an Empty named to what you want your system named as a child under an object with 
the `Anatomy Organizer` component
	- In the case of `CatCollection`, this is the `AnatomyCollection` object.
2. Add the `Body System` script as a component under it
3. Populate the `SystemInteractable` with your model of the system (made with [Adding a New Model](#adding-a-new-model))
4. Pupulate the array of parts (also made with [Adding a New Model](#adding-a-new-model))

## Improvements

- There is no start menu or settings, however, those would be great next 
additions.
- Since my focus was on the game and the programming, the models are lacking
and could use some polishing.
- If a part list is blank, do not show the UI

## Known Bugs

- If you quickly switch models before the previous model has finished returning to
its anchor point, it will show as floating/frozen next time it is selected.

## Turning Scans into Models

I succeed in turning one `DICOM` scan into a 3D model. This can be seen with the `Skull` model.
If you are able to aquire these scans reliably, it may be a good way to get accurate models.

The process was using [Slicer](https://www.slicer.org/). [How to work with DICOM](https://slicer.readthedocs.io/en/latest/user_guide/modules/dicom.html)

Presentation on going from `DICOM` to `stl` : [presentations](https://www.slicer.org/wiki/Documentation/Nightly/Training#Segmentation_for_3D_printing)

After I got a rough 3D model, I decimated and cleaned it up in [Blender](https://www.blender.org/)
