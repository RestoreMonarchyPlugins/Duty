# Duty  
Effectively manage and prevent abuse of staff members.

## Features
* Allows you to create multiple staff groups with different settings.
* Allows you to set additional restrictions to prevent abuse, for example, F1-F7.
* Integrated with Discord embeds when duty starts or ends, including a summary and more.
* Comes with an optional Duty UI.

## Credits
* **[tonislcs](https://github.com/tonislcs)** for making this plugin
* **soer** for making the UI

## Workshop
[3279408506](https://steamcommunity.com/sharedfiles/filedetails/?id=3279408506) - DutyUI

## Commands
* **/duty** - List all available duty groups that the calling player has permission to use.
* **/duty \<group\>** – Start or stop duty as a specific group.
* **/duty off** - Stop the active duty.

## Permissions
```xml
<Permission Cooldown="0">duty</Permission>
<!-- You can set the staff role permission in the configuration. e.g. -->
<Permission Cooldown="0">duty.admin</Permission>
<Permission Cooldown="0">duty.moderator</Permission>
```

## Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<DutyConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <UI Enabled="true" EffectID="59501" />
  <DutyGroups>
    <DutyGroup Name="Admin">
      <PermissionGroup>Admin</PermissionGroup>
      <Permission>duty.admin</Permission>
      <Settings>
        <GodMode>true</GodMode>
        <Vanish>true</Vanish>
        <AdminBlueHammer>true</AdminBlueHammer>
        <AdminFreecam>true</AdminFreecam>
        <AdminEsp>true</AdminEsp>
        <AdminBuilding>true</AdminBuilding>
        <BlockDamageToPlayers>false</BlockDamageToPlayers>
        <BlockStructureDamage>false</BlockStructureDamage>
        <BlockBarricadeDamage>false</BlockBarricadeDamage>
        <BlockStorageInteraction>false</BlockStorageInteraction>
        <BlockItemPickup>false</BlockItemPickup>
      </Settings>
    </DutyGroup>
    <DutyGroup Name="Moderator">
      <PermissionGroup>Moderator</PermissionGroup>
      <Permission>duty.moderator</Permission>
      <Settings>
        <GodMode>true</GodMode>
        <Vanish>true</Vanish>
        <AdminBlueHammer>false</AdminBlueHammer>
        <AdminFreecam>true</AdminFreecam>
        <AdminEsp>true</AdminEsp>
        <AdminBuilding>true</AdminBuilding>
        <BlockDamageToPlayers>true</BlockDamageToPlayers>
        <BlockStructureDamage>true</BlockStructureDamage>
        <BlockBarricadeDamage>true</BlockBarricadeDamage>
        <BlockStorageInteraction>true</BlockStorageInteraction>
        <BlockItemPickup>true</BlockItemPickup>
      </Settings>
    </DutyGroup>
  </DutyGroups>
  <Discord Enabled="false">
    <DutyStarted Enabled="true">
      <WebhookUrl>YOUR_WEBHOOK_URL</WebhookUrl>
      <Embeds>
        <Embed>
          <Title>{character_name} started {duty_name} duty</Title>
          <Thumbnail Url="{avatar_url}" />
          <Fields>
            <Field Name="Steam ID" Value="`{steam_id}`" Inline="true" />
            <Field Name="Steam Name" Value="[{steam_name}](https://steamcommunity.com/profiles/{steam_id})" Inline="true" />
            <Field Name="Date" Value="&lt;t:{date}&gt;" Inline="true" />
            <Field Name="Group" Value="{duty_name}" Inline="true" />
            <Field Name="Permission" Value="`{permission}`" Inline="true" />
            <Field Name="Position" Value="`{position_x} {position_z} {position_y}`" Inline="true" />
          </Fields>
          <Footer Text="{server_name}" />
          <WithCurrentTimestamp>true</WithCurrentTimestamp>
          <ColorHex>#00FF00</ColorHex>
        </Embed>
      </Embeds>
    </DutyStarted>
    <DutySummary Enabled="true">
      <WebhookUrl>YOUR_WEBHOOK_URL</WebhookUrl>
      <Embeds>
        <Embed>
          <Title>{character_name} {duty_name} duty summary</Title>
          <Description>Commands Executed: ```{commands_executed}```</Description>
          <Thumbnail Url="{avatar_url}" />
          <Fields>
            <Field Name="Steam ID" Value="`{steam_id}`" Inline="true" />
            <Field Name="Steam Name" Value="[{steam_name}](https://steamcommunity.com/profiles/{steam_id})" Inline="true" />
            <Field Name="Duration" Value="{time} seconds" Inline="true" />
            <Field Name="Started At" Value="&lt;t:{time_started}&gt;" Inline="true" />
            <Field Name="Ended At" Value="&lt;t:{time_ended}&gt;" Inline="true" />
            <Field Name="Position" Value="`{position_x} {position_z} {position_y}`" Inline="true" />
          </Fields>
          <Footer Text="{server_name}" />
          <WithCurrentTimestamp>true</WithCurrentTimestamp>
          <ColorHex>#ff0000</ColorHex>
        </Embed>
      </Embeds>
    </DutySummary>
    <DutyCommandLog Enabled="true">
      <WebhookUrl>YOUR_WEBHOOK_URL</WebhookUrl>
      <Embeds>
        <Embed>
          <Title>{character_name} executed command on duty</Title>
          <Thumbnail Url="{avatar_url}" />
          <Fields>
            <Field Name="Steam ID" Value="`{steam_id}`" Inline="true" />
            <Field Name="Steam Name" Value="[{steam_name}](https://steamcommunity.com/profiles/{steam_id})" Inline="true" />
            <Field Name="Command" Value="`{command}`" Inline="true" />
          </Fields>
          <Footer Text="{server_name}" />
          <WithCurrentTimestamp>true</WithCurrentTimestamp>
          <ColorHex>#ff0000</ColorHex>
        </Embed>
      </Embeds>
    </DutyCommandLog>
  </Discord>
</DutyConfiguration>
```


