function chain_yes ()
	api:makeDialog(api:Dialog("Well that's great then!\nI love you too."))
end

function chain_no ()
	api:makeDialog(api:Dialog("Well that's just rude."))
end

api:makeDialog(api:Dialog("Hello player!"), api:Dialog("I come from the world of lua."), api:Branch("Do you love me?", "Yes", "Nope lol", chain_yes, chain_no))
