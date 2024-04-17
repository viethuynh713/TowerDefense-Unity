# Mythic Empire - Unity Client

## Description
The 3D tower defense game project by our team is a real-time strategy game where players take on the role of leaders. The players will be tasked with devising strategies to build suitable towers for defense and deploy enemy attack waves. In the game, players will have the opportunity to design and build various types of buildings and military bases, defensive lines, and deploy enemy attack waves with a variety of cards that players possess.

This project is developed using Unity, utilizing SignalR technology to establish direct connections between players. The database is stored using MongoDB, and ASP.NET is used to build the backend for the application.

## Video Demo

Demo Video: [https://www.youtube.com/watch?v=RXJoQbxcVh4]

[![Video Thumbnail](https://img.youtube.com/vi/RXJoQbxcVh4/maxresdefault.jpg)](https://www.youtube.com/watch?v=RXJoQbxcVh4)

## Gameplay

### Main Lobby Interface

<center>
<a href="https://i.ibb.co/87ZF7c7/Lobby.png"><img src="https://i.ibb.co/87ZF7c7/Lobby.png" alt="Lobby" border="0" width="1000"></a>
</center>
  
Key Features:
- Shop: Players buy, sell their favorite cards.
- Adventure: Play mode against AI,  players will practice and compete against AI programmed with unique strategies and attack patterns.
- Arena: Play mode against other players, players will compete against each other to improve their rankings.
- Gacha: Players use their resources for a chance to receive rare cards.

### Card System

To start battles, players must have their own cards to build suitable strategies.

#### Types of Cards

<center>
<table>
    <tr>
        <td align="center"><img src="https://i.ibb.co/zXMH0rz/golem-common-card.png" alt="Image 1" width="300"><br><strong>Monster Card</strong></td>
        <td align="center"><img src="https://i.ibb.co/GczYHdm/energy-common-card-1.png" alt="Image 2" width="300"><br><strong>Tower Card</strong></td>
        <td align="center"><img src="https://i.ibb.co/5G6t0fY/toxic-common-card.png" alt="Image 3" width="300"><br><strong>Support Card</strong></td>
    </tr>
</table>
</center>

There are 3 main types of cards: tower cards, monster cards, and support cards:
- Tower Cards: allow building 1 tower unit on the battlefield for defense or support.
- Monster Cards: summon 1 attacking unit to the enemy's fortress.
- Support Cards: Provide support effects within a certain range such as healing, freezing, speeding up...

#### Rarity of Cards

Each card will have rarity corresponding to increasing strength as follows: Common, Rare, Mythic, Legend

<center>
<table>
    <tr>
        <td align="center"><img src="https://i.ibb.co/VvdkwVF/dog-common-card.png" alt="Image 1" width="300"><br><strong>Common Card</strong></td>
        <td align="center"><img src="https://i.ibb.co/GvFpMB2/dog-rare-card.png" alt="Image 2" width="300"><br><strong>Rare Card</strong></td>
        <td align="center"><img src="https://i.ibb.co/NCM4DTx/dog-mythic-card.png" alt="Image 3" width="300"><br><strong>Mythic Card</strong></td>
        <td align="center"><img src="https://i.ibb.co/Pmw722x/dog-legend-card.png" alt="Image 4" width="300"><br><strong>Legend Card</strong></td>
    </tr>
</table>
</center>

#### Card Levels

In addition to finding high rarity cards to increase strength, players can also upgrade cards to make them stronger, enhancing the strength of their lineup.

<center>
    <div>
        <img src="https://i.ibb.co/J35J4D3/Group-61.png" alt="Image 1">
        <h2>Increasing card level</h2>
    </div>
</center>

### In-Game Interface

When starting a match, the map will be initialized with obstacles such as trees, players cannot place cards on those obstacles.

<center>
<a href="https://i.ibb.co/5WrC5pX/start-gameplay.png"><img src="https://i.ibb.co/5WrC5pX/start-gameplay.png" alt="Lobby" border="0" width="1000"></a>
</center>

Players drag and drop cards from the card bar below to place cards on the battlefield. Players must build strong defensive strategies against enemy attack waves while organizing attacks on opponents to achieve victory.

<center>
<a href="https://i.ibb.co/Vt7j4KF/ingame.png"><img src="https://i.ibb.co/Vt7j4KF/ingame.png" alt="Lobby" border="0" width="1000"></a>
</center>

## Requirements
- Unity 2021.3.1f1
- SignalR
- MongoDB version 6
- ASP.NET version 6

## Installation
1. Clone the repository.
2. Run using Unity.
3. Run the necessary backend server and real-time server projects. Make sure you have installed and configured them correctly.
   - Backend project: [https://github.com/viethuynh713/TowerDefense-Backend]
   - Real-time project: [https://github.com/viethuynh713/TowerDefense_Realtime]
4. Configure the paths to the projects in the `NetworkingConfig.cs` file. Below is an example of how to configure:
   ```csharp
   // NetworkingConfig.cs
   public class NetworkingConfig {
       public const string ServiceURL = "http://localhost:5000/api";
       public const string RealtimeURL = "ws://localhost:8080"; 
   }

- ServiceURL is the path to the backend server.
- RealtimeURL is the path to the real-time server.
  
## Contribution
We welcome contributions from the community. Please create a Pull Request to contribute to this project.

## Author
Contact: viethuynh713@gmail.com


