# Project Management Editor Window
This readme exists to help keep the Github discussion ``// ToDo: (maybe, see below) create discussion`` explaining how to use this.

[Github Markdown Guide](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)

<mark>@Gerold I feel like we could use readmes like this for some modules</mark>, we can then easily refer to it on github, so long as we make sure it looks nice there.
Later, maybe we can look into using Obsidian as previously discussed

## Known Bugs
This should probably just be in issues or something, but could be useful as an internally used list of things to keep an eye out for because I think I fixed them.
- [ ] Invalid paths cause the entire editor to fail to display, meaning you can't fix the path without finding the object and editing it in place.

## Instructions
### Usage
How to use the Editor, should be understandable to non Unity users.

### Making a new tab
How to make a new tab for the Editor Window.

## Ideas
### Needed Features
Request features here or reply on discussion I guess?
- [ ] ParrelSync tab?

### Wanted Features
- [x] Decoupled tabs
- [ ] Toggleable sidebars, already half implemented
- [ ] Resizable tabs, header/sidebar
- [ ] Better tree view, maybe [Unity's Tree View?](https://docs.unity3d.com/Manual/TreeViewAPI.html) 
  - In the prefabs tab, for example each column could display an ability with a reference to its AbilityParameter SO for easy swapping later when we handle ability upgrading, etc.
  - A basic version was implemented and scrapped in favor of the current GroupList approach. This was the main motivation behind a TreeNode structure in the first place, so hopefully not much effort required.
    - Would need Trees to be able to handle heterogeneous types(?). 
    - Maybe require an interface that displays the object
      - Separate interfaces for Data/View like with tabs, or just use them with it.

### Misc Ideas

- [ ] Reorderable tabs. 
- [ ] Maybe look into creating dockable child windows in Unity?

# ToDo (to be used internally)
- [ ] Get Prefabs tab working, solution will likely solve MultiplayerSettingsTab not using custom inspector.
- [ ] Test by making a fresh instance of ProjectManagerSettings and initializing it, make sure it does so without having to mess with it.
- [ ] Figure out if we want ```ProjectManagementSettingsEditor.cs```
- [ ] Finish summaries and commenting