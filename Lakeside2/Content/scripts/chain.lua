function chain_yes ()
	api:makeChain(api:SDialog("Well that's great then!\nI love you too."))
end

function chain_no ()
	api:makeChain(api:SDialog("Well that's just rude."))
end

function chain_pos ()
	local x = api.player:getTileLocation().X
	if x < 2 then
		api:makeChain(api:SDialog("Talk to me when your X is >= 2"))
	else
		api:makeChain(api:SDialog("Perfect! Your X is high enough :)"))
	end
end

-- test attaching LuaFunction callbacks to text boxes
--api:makeChain(api:SDialog("Let me check your position...", chain_pos))

-- test SBranch
-- api:makeChain(api:SDialog("Hello player!"), api:SDialog("I come from the world of lua."), api:SBranch("Do you love me?", "Yes", "Nope lol", chain_yes, chain_no))

-- test basic SMove
--api:makeChain(api:SDialog("Eww, you smell!\nGet away from me!"), api:SMove(me, 0, 1), api:SDialog("Don't talk to me again!"))
--api:makeChain(api:SDialog("Cya"), api:SMove(me, 5, 0), api:SDialog("LOL"))

-- test SMove with pathfinding
api:makeChain(api:SMove(me, LuaAPI.makeVector2(10, 8)))