if Flags.getIntFlag("feed_gumshoe") == 2 then
	l:makeChain(
		l:SDialog("It's an ad for my favorite sports team, the Milwaukee Bucks."),
		l:SNPCDialog(l:getEntity("tv"), "Open any bag of cheese curds today and you could win tickets to see the Milwaukee Bucks!"),
		l:SNPCDialog(l:getEntity("tv"), "No purchase necessary. See back of bag for details."),
		l:SDialog("Wow! I should check to see if I won tickets. I love the Milwaukee Bucks.")
	)
	Flags.setIntFlag("feed_gumshoe", 3)
end