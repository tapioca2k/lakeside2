function chain_yes ()
	api:makeDialog(api:Dialog("Well that's great then!\nI love you too."))
end

function chain_no ()
	api:makeDialog(api:Dialog("Well that's just rude."))
end

function chain_pos ()
	local x = api.player:getTileLocation().X
	if x < 2 then
		api:makeDialog(api:Dialog("Talk to me when your X is >= 2"))
	else
		api:makeDialog(api:Dialog("Perfect! Your X is high enough :)"))
	end
end

api:makeDialog(api:Dialog("Let me check your position...", chain_pos))

-- api:makeDialog(api:Dialog("Hello player!"), api:Dialog("I come from the world of lua."), api:Branch("Do you love me?", "Yes", "Nope lol", chain_yes, chain_no))
