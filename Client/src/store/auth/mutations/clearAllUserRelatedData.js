
export default function clearAllUserRelatedData(state) {
	state.user = null;
	state.roles = ["Unregistered"];
	store.state.categories.root = null;
	store.state.categories.all = null;
	store.state.menu.namedMenuItems = null;
	store.state.sections.allSections = null;
	store.state.admin.showDeletedElements = false;
}
