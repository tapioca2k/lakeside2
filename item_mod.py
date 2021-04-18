import json, sys

item_file = 'Lakeside2/Content/items.json'

j = ''
with open(item_file, 'r') as f:
    j = json.loads(f.read())
print('loaded %s items' % (len(j), ))

def find_item(name):
    check = [i for i in j if i['name'] == name]
    if len(check) == 0:
        return None
    else:
        return check[0]

def save_items():
    with open(item_file, 'w') as f:
        f.write(json.dumps(j))

while True:
    save_items()
    choice = input('1: add item 2: del item 3: quit | ')
    if choice == '1':
        name = input('name: ')
        description = input('description: ')
        filename = input('filename: ')
        new_item = {'name': name,
                    'description': description,
                    'filename': filename}
        if find_item(name):
            print('Item already exists with that name')
        else:
            j.append(new_item)
    elif choice == '2':
        name = input('name: ')
        check = find_item(name)
        if check:
            j.remove(check)
        else:
            print('No item exists with that name')
    elif choice == '3':
        sys.exit()
    
            
