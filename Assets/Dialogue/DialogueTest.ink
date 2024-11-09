// Define variables at the start
VAR agent_name = "Agent Black"
VAR mission_status = "not_triggered"

// Starting point of the conversation
-> agent_arrival

== agent_arrival ==
Hi, I'm {agent_name} I need a room for tonight, something that ensures privacy and aligns with my usual preferences.

* [Certainly, any preferences?] -> preferences
* [We have a special offer.] -> special_offer

== preferences ==
Something with a view of the "moonlight," away from noise and distractions, would be ideal for a peaceful stay.

* [We have "midnight blue" room.] -> midnight_blue
* [All rooms have great views.] -> general_view

== special_offer ==
I'm interested. Tell me more about this offer and whether it includes any additional perks.

* [With "golden hour" perks.] -> golden_hour
* [Ideal for "half past two."] -> half_past_two

== midnight_blue ==
Excellent. Is it secure? I want to be certain there are no unexpected interruptions.

* [Absolutely, maximum privacy.] -> confirm_exit
* [You'll find it most discreet.] -> confirm_exit

== general_view ==
I prefer something more... exclusive, suited to someone with specific requirements for discretion.

* [Our "midnight blue" fits.] -> midnight_blue
* [We offer extra privacy.] -> confirm_exit

== golden_hour ==
Sounds suitable. Any extras? Perhaps something that adds to the ambiance or privacy?

* [Access to "silver feather."] -> confirm_exit
* [Free "blue case" service.] -> confirm_exit

== half_past_two ==
Just what I needed, perfectly timed. I trust all details are arranged in advance.

* [Proceed with booking?] -> confirm_exit
* [Any additional requests?] -> additional_requests

== additional_requests ==
Ensure the exits are accessible, with minimal staff presence for complete control.

* [Understood, all is set.] -> end_conversation
* [Security will be informed.] -> end_conversation

== confirm_exit ==
Thank you for your assistance. I expect everything to proceed smoothly, without disruptions.

* [Always a pleasure to help.] -> end_conversation
* [We're here for you anytime.] -> end_conversation

== end_conversation ==
~ mission_status = "triggered"
-> END
