﻿<template>
	<div class="material-form q-gutter-y-xs">
		<q-input
			class="material-form__name"
			v-if="canEditName"
			ref="name"
			v-model="material.name"
			:label="$tl('name')"
			:rules="rules.name"
		>
			<!--  <template v-slot:prepend>
          <q-icon name="fas fa-signature"/>
        </template>-->
		</q-input>

		<q-input
			class="material-form__title"
			ref="title"
			v-model="material.title"
			:label="$tl('title')"
			:rules="rules.title"
		>
			<!-- <template v-slot:prepend>
         <q-icon name="fas fa-heading"/>
       </template>-->
		</q-input>

		<q-input
			class="material-form__sub-title q-mb-sm"
			ref="subTitle"
			v-if="isSubTitleEditable"
			v-model="material.subTitle"
			type="textarea"
			autogrow
			:label="$tl('subTitle')"
			:rules="rules.subTitle"
		>
			<!-- <template v-slot:prepend>
         <q-icon name="fas fa-info"/>
       </template>-->
		</q-input>

		<SunEditor
			bottomSlots
			class="material-form__text-editor"
			content-class="material__text"
			:toolbar="editorToolbar"
			:rules="rules.text"
			ref="htmlEditor"
			v-model="material.text"
		/>

		<q-select
			class="material-form__tags"
			v-model="material.tags"
			bottom-slots
			use-input
			use-chips
			multiple
			:label="$tl('tags')"
			hide-dropdown-icon
			input-debounce="0"
			new-value-mode="add-unique"
		>
			<template v-slot:prepend>
				<q-icon :name="$iconsSet.MaterialForm.tags" />
			</template>
		</q-select>

		<CategoriesInput
			:categoriesNames="categoriesNames"
			v-model="material.categoryName"
			showIcon
			showIcons
			:label="$tl('selectCategory')"
		/>

		<q-input
			class="material-form__settings-json"
			v-if="canEditSettingsJson"
			ref="settingsJson"
			type="textarea"
			v-model="material.settingsJson"
			clearable
			autogrow
			:label="$tl('settingsJson')"
			:rules="rules.settingsJson"
		/>

		<div class="material-form__block-comments">
			<q-checkbox
				:toggle-indeterminate="false"
				v-if="canBlockComments"
				ref="isCommentsBlocked"
				v-model="material.isCommentsBlocked"
				:label="$tl('blockComments')"
			/>
		</div>

		<div class="material-form__hide">
			<q-checkbox
				:toggle-indeterminate="false"
				v-if="canHide"
				ref="isHidden"
				v-model="material.isHidden"
				:label="$tl('hide')"
			/>
		</div>
	</div>
</template>

<script>
import { htmlTextSizeOrHasImage } from "utils";
import { isJson } from "utils";

function createRules() {
	return {
		name: [
			value =>
				!value ||
				/^[a-zA-Z0-9-]+$/.test(value) ||
				this.$tl("validation.name.allowedChars"),
			value =>
				!value ||
				!/^[0-9]+$/.test(value) ||
				this.$tl("validation.name.numberNotAllowed"),
			value =>
				!value || value.length >= 3 || this.$tl("validation.name.minLength"),
			value =>
				!value ||
				value.length <= config.DbColumnSizes.Materials_Name ||
				this.$tl("validation.name.maxLength")
		],
		title: [
			value => !!value || this.$tl("validation.title.required"),
			value => value.length >= 3 || this.$tl("validation.title.minLength"),
			value =>
				value.length <= config.DbColumnSizes.Materials_Title ||
				this.$tl("validation.title.maxLength")
		],
		subTitle: [
			value =>
				!value ||
				value.length <= config.DbColumnSizes.Materials_SubTitle ||
				this.$tl("validation.subTitle.maxLength")
		],
		text: [
			value => !!value || this.$tl("validation.text.required"),
			value =>
				htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5) ||
				this.$tl("validation.text.htmlTextSizeOrHasImage")
		],
		settingsJson: [
			value =>
				!value ||
				isJson(value) ||
				this.$tl("validation.settingsJson.jsonFormatError")
		]
	};
}

export default {
	name: "MaterialForm",
	props: {
		material: {
			type: Object,
			required: true
		},
		categoriesNames: {
			type: String,
			required: false,
			default: "Root"
		}
	},
	data() {
		return {
			start: true
		};
	},
	computed: {
		hasError() {
			return (
				this.$refs.title.hasError ||
				this.$refs.htmlEditor.hasError ||
				!this.material.categoryName ||
				this.$refs.description?.hasError ||
				this.$refs.name?.hasError
			);
		},
		canEditName() {
			return (
				this.$store.state.auth.roles.includes("Admin") &&
				this.category?.isMaterialsNameEditable
			);
		},
		canHide() {
			return this.category?.categoryPersonalAccess?.MaterialHide;
		},
		canEditSettingsJson() {
			return this.category?.categoryPersonalAccess?.MaterialEditSettingsJson;
		},
		canBlockComments() {
			return this.category?.categoryPersonalAccess?.MaterialBlockCommentsAny;
		},
		categoryTitle() {
			return this.category?.title;
		},
		category() {
			return this.$store.getters.getCategory(this.material.categoryName);
		},
		isSubTitleEditable() {
			return this.category?.isMaterialsSubTitleEditable;
		}
	},
	methods: {
		validate() {
			this.$refs.name?.validate();
			this.$refs.title.validate();
			this.$refs.subTitle?.validate();
			this.$refs.htmlEditor.validate();
			this.$refs.settingsJson?.validate();
		}
	},
	beforeCreate() {
		this.rules = createRules.call(this);
		this.editorToolbar = JSON.parse(config.Editor.MaterialToolbar);
		this.$options.components.CategoriesInput = require("comp").CategoriesInput;
	}
};
</script>

<style lang="scss">
.material-form__menu-item {
	margin: 0;
	padding: 0;
}

.material-form__text-editor {
	.q-editor__content {
		height: 45vh;
		resize: vertical;
	}
}
</style>
