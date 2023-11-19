# Interest Aggregator
This is an Azure Functions program that checks the RSS feeds for games that you are interested in across Reddit and Steam for updates, key-words, or particular users posting. It then emails the highlights of those RSS feeds to you.

# Features
## Games, Key-Words and Users
You can specify the games, key-words or users that you want to filter on by adding them to a YAML file. The program will run periodically at 10PM UTC and check the RSS feeds for any new or updated posts that match your criteria. The program will then send you an email with a summary of the relevant posts, including the title, link, and a short excerpt of the content.

## Football Fixtures
Additionally, leveraging the FixtureFetcher, integration to follow the diary of your favourite football/soccer team is built-in, and can be specified.

# Installation
Pre-requisites:
- A fully setup Azure Email service
To install, pull the repository and change the email preferences located in AzureEmailManager.cs to use your to/from address and to supply your own Azure Email servie API key.

Then, update the YamlFeedList.yml to contain whatever Reddit/Steam feeds you are interested in receiving updates for, then publish to Azure Functions.
