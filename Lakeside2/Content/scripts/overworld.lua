function overworld_go()
	l:goToOverworld()
end

l:makeChain(l:SBranch("Go to the world map?", "Yes", "No", overworld_go, nil))