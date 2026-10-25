# Translating Companion & Party Limits Expanded

Companion & Party Limits Expanded ships in English only. Every piece of text the mod adds can be translated, and translations are welcome as separate mods that require Companion & Party Limits Expanded. You do not need to change or copy any of its files.

## What can be translated

All of the mod's own text is in `ModuleData/Languages/EN/cple_strings_en.xml`: the Mod Configuration Menu (MCM) page with every group, setting name and hint, and the word *Unlimited* shown on the clan screen. Use that file as your template.

The rest of the clan screen's companion and party counts, such as **Companions** and **Parties**, is the base game's own text. The game already translates it, so it is not in the file.

## Making a translation module

A translation module contains only text files. Example for Russian:

```
CompanionPartyLimitsExpanded_RU/
├─ SubModule.xml
└─ ModuleData/
   └─ Languages/
      └─ RU/
         ├─ language_data.xml
         └─ cple_strings_ru.xml
```

`SubModule.xml`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Module>
  <Name value="Companion &amp; Party Limits Expanded - Russian"/>
  <Id value="CompanionPartyLimitsExpanded_RU"/>
  <Version value="v1.0.0"/>
  <SingleplayerModule value="true"/>
  <MultiplayerModule value="false"/>
  <DependedModules>
    <DependedModule Id="CompanionPartyLimitsExpanded" DependentVersion="v1.0.0"/>
  </DependedModules>
  <DependedModuleMetadatas>
    <DependedModuleMetadata id="CompanionPartyLimitsExpanded" order="LoadBeforeThis" version="v1.0.0"/>
  </DependedModuleMetadatas>
  <SubModules/>
</Module>
```

Give your module its own `Id`. Set `DependentVersion` and `version` to the Companion & Party Limits Expanded version your file was made from.

`language_data.xml`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<LanguageData id="Русский">
  <LanguageFile xml_path="RU/cple_strings_ru.xml" />
</LanguageData>
```

`cple_strings_ru.xml` is a copy of `cple_strings_en.xml` with the language tag changed to the same name and every `text` value translated:

```xml
<tags>
  <tag language="Русский" />
</tags>
```

### Language names

The language name must match the game's exactly, including accents and capital letters:

| Language | Name to use |
|---|---|
| Brazilian Portuguese | `Português (BR)` |
| Chinese (Simplified) | `简体中文` |
| Chinese (Traditional) | `繁體中文` |
| French | `Français` |
| German | `Deutsch` |
| Italian | `Italiano` |
| Japanese | `日本語` |
| Korean | `한국어` |
| Latin American Spanish | `Español (LA)` |
| Polish | `Polski` |
| Russian | `Русский` |
| Turkish | `Türkçe` |

For a language the game does not list here, copy the name from the `language_data.xml` of a mod that adds that language to the game.

## Rules for the text

- **Keep every `id` exactly as it is.** Only change `text`.
- **Save the file as UTF-8.**
- **Escape XML characters** inside `text`: `&` as `&amp;`, `<` as `&lt;`, `>` as `&gt;` and `"` as `&quot;`.
- **Missing lines are fine.** Any ID left out of your file shows in English, so a partial translation still works.

## Testing

1. Enable your module after Companion & Party Limits Expanded in the launcher and choose your language in the game's options.
2. Check the **Companion & Party Limits Expanded** page in MCM under **Options > Mod Options**, and the clan screen with an **Unlimited** switch on.
3. A line that still shows in English means its ID is missing or misspelled in your file. If the game log reports an XML error, check the escaping rules above.

The translation only changes text. It adds nothing to saves, so it can be added or removed at any time.

## Keeping up with updates

IDs are kept from one version to the next, so a translation keeps working after Companion & Party Limits Expanded updates. New text gets new IDs, which show in English until your translation adds them. The [changelog](CHANGELOG.md) lists added, changed and removed IDs for each version.

## Permission and credit

You may publish a translation of Companion & Party Limits Expanded as a separate mod, on Nexus Mods or elsewhere, under terms of your choice, as long as it contains only translated text and no code or other files from Companion & Party Limits Expanded. Please list Companion & Party Limits Expanded as a requirement and credit it in your mod's description:

    Translation of Companion & Party Limits Expanded by AtanuDebbarma
    https://github.com/AtanuDebbarma/CompanionPartyLimitsExpanded
