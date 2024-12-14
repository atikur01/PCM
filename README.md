# Arbitrary Collection Management System

It's a pet project, which was done as a requirement of [Itransition](https://www.itransition.com) internship, that includes all basic to advanced features of a **_personal collections showcasing platform_**. \
Build with - .NET 8 (MVC), Entity Framework\
The live project can be found [here](https://arbitrary-collection-mgmt.azurewebsites.net/).

> A few features include:
1. EAV model where registered users/admins can create a collection with an infinite number of attributes of multiple types that are available across all items belonging to that collection.
    - Items can have multiple tags, and the user can create new tags on the go, which are stored on the system and available for future uses.
2. Real-time likes & comments update feature with SignalR.
3. Collection images are stored in the cloud instead of local space.
4. SQL Full-Text search feature, which has the search ability across the entire site.
     - Search results include tags, comments, collections, and items.
5. Has an integration with Salesforce & Jira.
     - Users can upload their data directly in Salesforce and also create & track support tickets in Jira from the app.
