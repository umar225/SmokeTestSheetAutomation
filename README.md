# Boardwise

### Product Overview ###

Boardwise is a platform for non-executive directors to connect, train and find board opportunities to advance their career.
System is divided in 2 main components [**Web App**](https://bitbucket.org/Together/coursewise/src/master/App/) and [**API**](https://bitbucket.org/Together/coursewise/src/master/Server/).

### Run Both (App and Api) ###
- Make sure you have installed docker, then inside the root directory you will run `docker-compose up`, It will run app on 5020 and api on 5000 port
- Some secrets are not avilable in API **appsettings.json**, you can contact your lead to get these secrets or get it from [s3 transformation butcket](https://s3.console.aws.amazon.com/s3/buckets/coursewise-transformation?region=eu-west-2&tab=objects) file name is appsecrets.json.

### System Diagram ###
All diagrams or media in this folder [**media**](https://bitbucket.org/Together/coursewise/src/master/Media/).
!["system diagram"](./Media/Flow%20Diagrams/Coursewise%20-%20Architecture%20Diagram%20-%20v4.drawio.png)

### Business Model Change ###
In the begining of this product was a courses marketplace and the name was also coursewise but is the second half of 2023 business model was changed and it became a platform for non-executive directors to connect, train and find board opportunities to advance their career. Also the name of product was updated to boardwise.

### v1 branch ###
The [**v1 branch**](https://bitbucket.org/Together/coursewise/src/v1/) contains the code for coursewise. We excluded coursewise code from master branch.

### Future of the project decided in Dec 2023 ###
In Dec 2023 development was put on hold. The plan was to resume the development with the implementation of Linkedin automation imitating the Phantom Buster i-e sending connection requests and messages.

### Proposal Document ###
[**Google Recaptcha v2 Proposal document**](https://theventurestudio.jira.com/wiki/spaces/CMD/pages/4058185733/Proposal+document+-+Recaptcha+implementation)

### Dependency Scanner Analysis ###
[**Dependency Scanner Issues Analysis**](https://docs.google.com/spreadsheets/d/1UY4S7xfOGD9U-DE2p-c4j4CFkEdTAsAxmiNb73LRK3c/edit#gid=0)
### Developers ###
Specific components details available in App and Server folders.