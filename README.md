# Autonomous_Drone
Sophmore Software engineering Project (SCRUM)

#Note
Originally meant to be an autonomous search and rescue drone. Due to legal restrictions, costs and practicality, 
our project was reassed to to be an autonomous land-based robot that searches for the selected indiviudal.

#Description
The purpose of this project was to learn how to properly and/or better work in small-medium sized groups on long term 
projects using the Agile SCRUM Methodology

Our project was an autonomous land-based robot that searches for a missing person on-campus. A user launches the client software and connects to a network shared by a remote server and the robot. The Client uploads an image of the individual to the remote server and starts the robot. The robot roams the area and searches for a distinguishable face. Upon detecting one it stops and uploads an image of the detected face to the server. Using Microsoft's facial recognition API, the server compares the detected face to the Client's uploaded image. If there is a 70% match or higher, the server sends the image of the detected face to the Client and confirms it as a match.

The drone was comprised of an arduino, 2 raspberry pi's, and a breadbox. The arduino handled the robots steering and collision
avoidance. One of the Raspberry PI's controlled the camera and facial recognition. The other Raspberry PI was responsible with 
network communications. Lasty, the breadbox connected the arduino and raspberry PI's to interface with each other.
