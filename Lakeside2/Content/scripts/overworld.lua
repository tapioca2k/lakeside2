function overworld_go()
	l:goToMap()
end

l:makeChain(l:SBranch("Go to the world map?", "Yes", "No", overworld_go, nil))