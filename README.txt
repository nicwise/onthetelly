
iplayeroffline / OnTheTelly
--------------------------------------

This is a MonoTouch application which is an offline client for the BBC iPlayer.

Requires:

XCode 3.x (Build for 3.0 SDK, runs on anything up to 4.0 beta (current version). 

MonoTouch 2.x (works with 4.x beta, and all in between)

MonoDevelop and Mono to go with MonoTouch


Also works on the iPad.

Credits
--------------------------------------

Thanks to Paul for releasing the source to iplayer-dl (http://po-ru.com/projects/iplayer-downloader/),
which the downloader is based on. I'm still staggered by the amount of time that has been put
into that application.

Images came from Glyphish : http://glyphish.com/


Issues
--------------------------------------

The application is fully functional, however as BBC does not allow for commercial (non-personal) 
use of iPlayer content (http://iplayerhelp.external.bbc.co.uk/help/about_iplayer/termscon - clause 12),
putting it into the App Store (or simlar) will result in a Cease and Desist, which is a waste of everyone's
time.

Downloading shows will not _ever_ work if you are outside of the UK (geo-ip restrictions on iPlayer)
or if you are in the UK, but on a 3G connection.

UPDATE: This code is nearly 12 months old! WOW! BBC shut down the iplayer feed, so I'd need to do some work on it
to get it going properly - Paul has doc'ed this here: 

http://po-ru.com/diary/bbc-fights-against-openness-again/

I might be able to get the key out, being this runs on an iPhone/iPod Touch, but I've not tried. Maybe later. 

Could be a good (or possibly VERY bad) example of how (not) to do MonoTouch. I'd use MonoTouch.Dialog if I write it again - 
would cut out about 80% of the code!

license
--------------------------------------

This is licensed under a modified MIT license. As below, however this application may not be commercially 
used (on the App Store or otherwise) as-is or without major modification. If in doubt, ask.

Think of it like a Creative Commons Attribution-Noncommercial-Share Alike 2.0 (http://creativecommons.org/licenses/by-nc-sa/2.0/uk/)

Some icons came from Glyphish's icon set:
http://glyphish.com/

Go buy the full ones. They are worth every cent.



The MIT License

Copyright (c) 2010 Nic Wise - nic.wise@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

No portions of this code are (C) British Broadcasting Corporation (BBC), however their
iPlayer is used as the source of data, and they do own that. Play nicely.