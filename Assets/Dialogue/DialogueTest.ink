// DEFINE VARIABLE - ONLY CHANGE VISITOR_NAME
VAR visitor_name = "Cognac"
VAR protagonist_name = ""
VAR dialogue_state = "start"
VAR talking = ""

// DIALOGUE STATE = START
-> visitor_arrival

== visitor_arrival ==
~ talking = visitor_name
I need a room for tonight.

* [>]

    Preferably the one with the AC.

    ** [Okay.] -> review_document
    ** [Sure.] -> review_document

// DIALOGUE STATE = REVIEW DOCUMENT
== review_document ==
~ dialogue_state = "get_document"
~ talking = visitor_name
Here's the document.

* [>] -> finish_document

== finish_document ==
~ dialogue_state = "finish_document"
~ talking = protagonist_name
Your document is fine, thank you.

* [>] -> give_card

// DIALOGUE STATE = GIVE CARD
== give_card ==
~ dialogue_state = "card"
~ talking = protagonist_name
Wait a moment, I'm getting your card.

* [x] -> closing

== closing ==
There you go.

* [>]
    ~ talking = visitor_name
    Thank you, {protagonist_name}.
    ** [>] -> end_conversation

// DIALOGUE STATE = END
== end_conversation ==
~ dialogue_state = "end"
-> END
