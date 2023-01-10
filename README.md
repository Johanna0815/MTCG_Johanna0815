# MTCG_Johanna0815
This HTTP/REST-based server is built to be a platform for trading and battling with and against each other in a magical card-game world.

First step was to setup the environment. As an IDE Microsoft Visual Studio, with an "Console App" Project started as a new Project. Programming Language C#, .net6 Version. Within this project there was x classes created (later removed and reorganized them). The Project also includes a second "new Project" created was a "NUnit Test Project" for the Unit Tests within the project. 

### Overall there are five SubProjects and 1 MainProject:

- MTCG_TheOrigin - MainProject
- MTCG_TheOrigin.Test - Tests/Unittests
- DataAccessLayer - DAL
- Model
- BusinessLayer - BL
- HttpServerLayer -HSL

I regretted very much having designed the project with subprojects. Because it turned out to be difficult for me to include the subprojects. However, I learned afterwards that you only have to integrate the subprojects into the MainProjekt (MTCG_TheOrigin <- in my case); to avoid circular dependence.

The server was created in class, except that I needed to rewrite for an 'TcpClientAsync'. As I was not able to fix my problem with including the BattleLogic from Subproject DAL, I excluded and morever ignored the Subprojects and the programm is less 'nice formatted' and less design patterns. 
But for me it was more important that the project runs. [MUST HAVES should be included. ]
- Uses C# or Java {one of them}
- Implements a server listening to incoming clients {diligent server is included}
- Implements multiple threads to serve client requests { yes, with keyword async the tasks run contemporaneous - asynchron}
- Does not use an HTTP helper framework {NO, indeed not.}
- Uses a Postgres Database for storing data {Yes, I use a docker container(image: postgres), where whenever the commands [docker start swe1db] [docker exec -it swe1db bash] [psql -U swe1user] are used the 'postgres'-Container runs. Postgres DB is connected and included in the project (Npgsql as a package in the Subprojcet.DA) for visual purpose I used DBeaver - named it mtcg_theorigin}
- Does not allow for SQL injection {emmm...NO?!}
- Does not use an OR-Mapping Library {NO, indeed not. Serialized and Desirialized with JsonSerializer.Deserialize||Serialize library[using System.Text.Json] and an Attributes [Serializable], [JsonInclude] <- detrimming the String.}
- Implements at least 20 Unit Tests {t.b.a}


#### Describes design
unique design I guess, boxing/unboxing, classes separate from the Main Project. lambda Functions, GenericTyps in form of Lists/ Dictionary used. 
Token Based Security. DB stores after an end of an GoBattle (not after each round.) It stores just Win and Loos. 
### POST
each request needs UserName and Password OR AccessToken
### GET
each request needs UserName and Password OR AccessToken
#### Describes lessons learned
t.b.a

#### Describes unit testing decisions
Tests separate with NUnit.Framework
works with json file and Postman. 

#### Describes unique feature
trade. t.b.a

#### Contains tracked time
tracked time; in a leap year I would have needed one more day. 

#### Contains link to GIT
https://github.com/Johanna0815/MTCG_Johanna0815



