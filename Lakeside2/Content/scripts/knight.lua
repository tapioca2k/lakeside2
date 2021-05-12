function talk_howdy()
	l:makeChain(l:SDialog("Howdy partner!\nWas that so hard?"))
	l:followPlayer(nil)
end

if l:isFollowingPlayer(me) then
	l:makeChain(l:SBranch("Want to to leave you alone? Say it.", "Howdy.", "I refuse.", talk_howdy, talk_refuse))
else
	l:makeChain(l:SDialog("Lovely weather we're having today!"))
end