SharpWit
========

C# 4.5 Wit.ai natural language interpreter
=========================

Developed by: Sam Kraaijveld

Uses: wit.ai (https://wit.ai/) and json.net

WPF project, but the code is interchangable with WinForms

=========================

Wit.ai is an online service that takes a natural language sentence, ie. 'I have a meeting tomorrow', and sends back data that can be easily interpreted by software, ie. 'intent: appointment, datetime: 2014-03-02T00:00:00.000+01:00'.

This application connects to your wit.ai instance, sends a sentence and retrieves the interpreted data. The data will then be parsed from json and cast into corresponding classes. Because of its modular design it can be used for anything really, feel free to take any part of the code. 

The gui is just for show, the various classes in 'Brain' as well, you should add your own more robust code that tailors your nlp needs. Read 'readme.txt' in the Brains folder for important information.

IMPORTANT: Open Vitals.NLP.NLP_Processing.cs and add your wit.ai access token, or the connection will fail. You can find it under 'Settings' in the wit.ai console.

A big thank-you to the developers over at wit.ai for making their fantastic service, and free-to-use at that :) You can find them here: https://wit.ai/


Sam.

=========================

Version history:

10.05.2014

- Added mandatory versioning, see https://wit.ai/docs/api#versioning for more details

02.03.2014

- Support for JSON arrays
- Support for voice; interpret your speech

01.03.2014	

- Connect with wit.ai and send/retrieve data
- Interpret JSON and cast into custom class
- Interpret intent and cast into corresponding class
- Basic custom string based on data as a proof-of-concept
