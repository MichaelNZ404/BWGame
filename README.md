
# TODO: 
- godhand movespeed is really slow when camera is directly over head
- pick up items with right click, throwing etc (cast down correctly, apply velocity of mouse movement)
- miracles
- lock actions to inside influence
- fix influence generartion
- create new buildings (workshop mechanic)

# NOTES: 
## DRAG FORWARD
- god hand is locked to screen edges which prevents drag from going past hand

## ROTATE AND PITCH:
- middle click rotate and pitch is centered on god hand
- edge of screen rotate and pitch are camera only

## GAMEPLAY:
- god hand has maximum distance from camera, just raycast through for target.
- no maximum range for grab, just becomes difficult to see
- detail decreases with zoom, influence ring should be visible from space.
- hard bounding box around island.
- ability to show info next to god hand (eg zoom with mouse icon, or the amount of wood being picked up)
- press S for villager debug info, has range (same as godhand?) NAME|ACTION|AGE LIFE FOOD
- double click terrain to fly over and zoom (with wind sounds)
- throwing projectiles with respect to perspective
- temple has enterance, creature pen, and worship sites

## MIRACLES:
- keybinds 1-5 to use miracles?

## SOUNDS:
- soft thud when grabbing terrain
- wind when high up.
- villagers laughing etc
- water noise when near ocean
- birds when near ground
- animal sounds
- footsteps of villagers
- rock rolling sound

## COOL FEATRURES:
- challenge room in temple / challenges.
- creature cave
- gods playground
- small map in center of temple

## CREATURE:
- discipline/pretting
- leashes
- AI

## Won't do (for now)
- left and right click zoom mode
- edge of screen rotate + pivot