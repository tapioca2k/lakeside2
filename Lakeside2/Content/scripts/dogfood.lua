function gumshoe_facetv()
	l:getEntity("gumshoe"):faceEntity(l:getEntity("tv"))
end

if Flags.getIntFlag("feed_gumshoe") == 1 then
	local gumshoe = l:getEntity("gumshoe")
	l:makeChain(
		l:SDialog("Yeah, that's where I left my food for Gumshoe. Cheese curds!"),
		l:SNPCDialog(gumshoe, "Bark! Bark!"),
		l:SMove(gumshoe, LuaAPI.makePoint(9, 3)),
		l:SFunction(gumshoe_facetv),
		l:SDialog("It seems like Gumshoe noticed something on TV.")
	)
	l:followPlayer(nil)
	Flags.setIntFlag("feed_gumshoe", 2)
elseif Flags.getIntFlag("feed_gumshoe") == 3 then
	l:makeChain(
		l:SDialog("There's a slip of paper at the bottom of the bag. It's covered in grease."),
		l:SDialog("It says..."),
		l:SDialog("I won! Two tickets to the Milwaukee Bucks! My favorite sports team!"),
		l:SDialog("I'm going to stop streaming and get to the stadium right away.")
	)
	Flags.setIntFlag("feed_gumshoe", 9999)
end