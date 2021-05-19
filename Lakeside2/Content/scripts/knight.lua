function talk_howdy()
	Flags.setBooleanFlag("said_howdy", true)
	l:makeChain(l:SDialog("Howdy partner! Was that so hard?"), 
				l:SDialog("I'm going into town. You should come visit me, seeing as we're friends now and all!"))
	l:followPlayer(nil)
end

if l:isFollowingPlayer(me) then
	l:makeChain(l:SBranch("Want to to leave you alone? Say it.", "Howdy.", "I refuse.", talk_howdy, talk_refuse))
else
	l:makeChain(l:SDialog("Lovely weather we're having today!"))
end