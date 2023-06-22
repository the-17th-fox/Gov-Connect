# Main Idea

GovConnect — is an app that connects government agencies with civilians via microservice RESTful WebAPI.

It allows state authorities to create global notifications (example: hurricane, storm, extreme tempreture, etc) and respond to the civilians reports (consequently, citizens have the opportunity to create them).

## Contents
1. [Features](#Features)

# Features

### Communications

#### For government authorities

| Feature | Description |
|     :---:      |     :---     |
| **Manage Classifications** (tags) | Classifications can be used by Civilians in their reports and by authorities for their Notifications. It could represent location, incident type, etc    |
| **Manage Notifications**       |  Notifications - some type of message, that is produced by state authority and can be seen by anyone. *Example: firedept. notifies civilians about any local emergency situation.* |
| **Check pending Reports** | Check reports that is temporally don't have a response |
|**Respond to any Report**|Pending reports can be answered|

#### For civilians

| Feature | Description |
|     :---:      |     :---     |
| **Create Reports** | Using description, header and the classifications they can describe what they are worried about |
| **Explore reports** | All reports are public, so every civilian can look at the reports and replies to them. |
| **Check Notifications** | Be awared of the state situation using Notifications system. |

> Every notification or report reply saves required users' information (depends on the endpoint requirements). For gov. authorities it could be: id, agency title. For civilian: id, first name, patronymic.

### Auth. services

#### For auth. services administrator

| Feature | Description |
|     :---:      |     :---     |
| **Manage users' accounts** | Accounts could be deleted, (un)blocked, etc |
| **Search civilians by passport information** | Using user's full name administrator can find all his information, provided to the server |

# Frameworks and Tools

| Tool | Description |
| :----: | ---- |
| `MediatR` | Handles controllers request to the DB. Is a part of CQRS architecture |
| `Identity` | **Handles registration and login processes** in accounts management microservices |
| `Redis` | Is used for **response caching**. Caching place could be configured using custom attribute |
| `Elastic Search` | Is used for **indexing** Reports and Notifications main properties. Also, it is configured for **searching them by their Header** |
| `SignalR` | Is used for **real-time viewing** of new Reports, Replies and Notifications |
| `Ocelot` | Is used for **Gateway operating**, handles **headers and claims transfering** |

#### System that keeps users info in Communications-DB consistent
| Tool | Description |
| :----: | ---- |
| `Kafka` | **Handles updates** of user's name and patronymic and **sends new info to the partition**. Also, can be called by Hangfire from another microservice to **read new records** |
| `Hangfire` | Running as a Hosted Service, every {time} **looks up for any updated account's info** (using Kafka) in the Kafka's partitions |

# Structure

Project consists of four main parts: 
### Communications Microservice
Is responsible for all form of interactions in context of communication between state authorities and civilians.

<details>
<summary>Inner structure</summary>

* API - contains controllers, microservice configuration, some viewmodels
* Infrastructure - contains repositories, db-context, utilities
* Application - implements CQRS pattern using MediatR, contains command, querries
* Core - is responsible for business requirements. Contains db-models, interfaces
* SignalR - contains signalR hubs and configuration
* Hangfire - contains hangfire interfaces and services implementation

</details>

_______
### Authorities Accounts Microservice
Handles all actions related to the authorities accounts

<details>
<summary>Inner structure</summary>

* API - contains controllers, microservice configuration, some viewmodels, middlewares
* Infrastructure - contains repositories, db-context, utilities
* Application - imeplement bussiness logic, contains viewmodels and services
* Core - is responsible for business requirements. Contains db-models, interfaces

</details>

_______
### Civilians Accounts Microservice
Handles all actions related to the civilians accounts. Also, contains and handles users' passports information.

<details>
<summary>Inner structure</summary>

* API - contains controllers, microservice configuration, some viewmodels, middlewares
* Infrastructure - contains repositories, db-context, utilities
* Application - imeplement bussiness logic, contains viewmodels and services
* Core - is responsible for business requirements. Contains db-models, interfaces

</details>

_______
### Gateway
Handles requests to the app, manages authentication and authorization, transfer required claims as headers down the request pipeline.
Implements custom authentication based on JWT.

Inner structure contains auth. handler and some other configuration.
_______
### SharedLib
Library which contains shared lib-projects, those is used by more than one microservice.

<details>
<summary>Inner structure</summary>

* Elastic Search - contains custom exceptions, configuration, interfaces and middlewares
* Exception Handler - contains global custom exceptions and exception handling middleware
* Kafka - contains configuration, interfaces, serializers and consumer+producer classes
* Redis - contains custom attribute, interfaces, implementations, utilities, configuration

</details>


# How To Run

To run this project you need to complete next steps.

1. Clone this repository using Git: 
```gitattributes
git clone https://github.com/the-17th-fox/Gov-Connect.git
```
2. In the /src/ folder using Docker, run docker-compose files:
```gitattributes
docker compose -f docker-compose.yml -f docker-compose.override.yml run -d
```
Now you can access the app using permitted and configured endpoints.
