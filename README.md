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
- Uses a Postgres Database for storing data {Yes, I use a docker container(image: postgres), where whenever the commands [docker start swe1db] [docker exec -it swe1db bash] [psql -U swe1user] are used the 'postgres'-Container runs. Postgres DB is connected and included in the project (Npgsql as a package in the Subprojcet.DA) for visual purpose I used DBeaver - named it mtcg_theorigin; then I switched to pgAdmin4 name of DB is mtcg_theOriginI, Why so? FUnfact: I forgot to reschedule the Port so firstly it did not work out with 2DB, but after sitching+ different POrt it worked.}
- Does not allow for SQL injection {emmm...NO?!}
- Does not use an OR-Mapping Library {NO, indeed not. Serialized and Desirialized with JsonSerializer.Deserialize||Serialize library[using System.Text.Json] and an Attributes [Serializable], [JsonInclude] <- detrimming the String.}
- Implements at least 20 Unit Tests 


#### Describes design
unique design I guess, boxing/unboxing, classes separate from the Main Project. lambda Functions, GenericTyps in form of Lists/ Dictionary used. 
Token Based Security. DB stores after an end of an GoBattle (not after each round.) It stores just Win and Loos. After the user registers, it gets an uid, increments by each new user, when 2 users have set an Deck, (each 4 cars) the can go for a batttle, in the meantime all others would have waited in a Queue. After finishing the battle, the DB has the entry of win and loos. 
### POST
each request needs UserName and Password OR AccessToken
### GET
each request needs UserName and Password OR AccessToken
#### Describes lessons learned
lessons learned. eamm Http Protocol, enpoints, WebserverLifeCircle, C# async Tasks, using an own created Webserver. 
NEVER delete Block of Code, before pulling/pushing/commiting to GitHuB. {I just use one of the TestProjects, but nearly all packages are in the other one; I deleted, But regreted it just a view moment afterwards, bc all tests failed, when I removed. So i just fixed it picking from github again, ..}
![image](https://user-images.githubusercontent.com/81578777/212503178-b154e6a1-7142-483c-b2e4-826200dad956.png)




#### Describes unit testing decisions
Tests separate with NUnit.Framework; XUnit. StepByStep as Curl would do it like Integration Tests.
works with json file and Postman. 

#### Describes unique feature
mandatory feature, after the winner has a win, it gets 2 coins. 

#### Contains tracked time
tracked time; in a leap year I would have needed one more day. 

#### Contains link to GIT
https://github.com/Johanna0815/MTCG_Johanna0815



