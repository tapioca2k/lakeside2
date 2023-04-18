if not Flags.getBooleanFlag("intro") then
	l:makeChain(
		l:SDialog("It all started one night, when pie was deep into lab resets and needed a break...")
	)
	Flags.setBooleanFlag("intro", true)
end