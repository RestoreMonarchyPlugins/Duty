# Duty
A duty plugin designed to effectively manage and prevent abuse by your staff members.

## Features
* Allows you to create many different staff roles.
* Allows you to set additional restrictions to prevent abuse, for example, F1-F7.
* Integrated with Discord embeds when duty starts or ends, including a summary and more.
* Comes with an optional Duty UI.

## Workshop
[3270578447](https://steamcommunity.com/sharedfiles/filedetails/?id=3270578447) - DutyUI

## Commands
* **/duty  \<group\>** – Allows the staff member go on or off duty
* 

## Permissions
```xml
<Permission Cooldown="0">duty</Permission>
```
> You can set the staff role permission in the configuration.

## Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<DutyConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <UIService>
    <UIEnabled>true</UIEnabled>
    <EffectID>59501</EffectID>
  </UIService>
  <DutyGroups>
    <DutyGroups>
      <DutyGroupName>Admin</DutyGroupName>
      <PermGroup>Admin</PermGroup>
      <Permission>duty.admin</Permission>
      <DutySettings>
        <GodMode>true</GodMode>
        <Vanish>true</Vanish>
        <AdminFreecam>true</AdminFreecam>
        <AdminEsp>true</AdminEsp>
        <AdminBuilding>true</AdminBuilding>
        <BlockDamageToPlayers>true</BlockDamageToPlayers>
        <BlockStructureDamage>true</BlockStructureDamage>
        <BlockBarricadeDamage>true</BlockBarricadeDamage>
        <BlockStorageInteraction>true</BlockStorageInteraction>
        <BlockItemPickup>true</BlockItemPickup>
      </DutySettings>
    </DutyGroups>
  </DutyGroups>
  <Discord Enabled="true">
    <DutyStarted Enabled="true">
      <WebhookUrl>YOUR_WEBHOOK_URL</WebhookUrl>
      <Embeds>
        <Embed>
          <Title>Player came on duty</Title>
          <Thumbnail Url="{thumbnail}" />
          <Fields>
            <Field Name="**Player**" Value="Steam Name: **[{name}](https://steamcommunity.com/profiles/{steam_id})** ({steam_id}) &#xA; Character Name: **{charactername}** " Inline="true" />
            <Field Name="**Duty Info**" Value=" Time: &lt;t:{date}:F&gt; &#xA; Duty Group: `{dutyname}`  &#xA; Has Permission `{permission}`" Inline="true" />
            <Field Name="**Player Position**" Value="X: `{positionx}` &#xA; Y: `{positiony}` &#xA; Z: `{positionz}` &#xA;" Inline="true" />
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
          <Title>Duty summary</Title>
          <Thumbnail Url="{thumbnail}" />
          <Fields>
            <Field Name="**Player**" Value="Steam Name: **[{name}](https://steamcommunity.com/profiles/{steam_id})** ({steam_id}) &#xA; Character Name: **{charactername}** " Inline="true" />
            <Field Name="**Duty Info**" Value="Started at: &lt;t:{timestarted}:F&gt; &#xA; Ended at: &lt;t:{timeended}:F&gt; &#xA; Total Seconds: {time} " Inline="true" />
            <Field Name="**Player Position**" Value="X: `{positionx}` &#xA; Y: `{positiony}` &#xA; Z: `{positionz}` &#xA;" Inline="true" />
            <Field Name="**Commands executed**" Value="{commands_executed}" Inline="true" />
          </Fields>
          <Footer Text="{server_name}" />
          <WithCurrentTimestamp>true</WithCurrentTimestamp>
          <ColorHex>#ff0000</ColorHex>
        </Embed>
      </Embeds>
    </DutySummary>
    <DutyCommands Enabled="false">
      <WebhookUrl>YOUR_WEBHOOK_URL</WebhookUrl>
      <Embeds>
        <Embed>
          <Title>DutyCommands</Title>
          <Thumbnail Url="{thumbnail}" />
          <Fields>
            <Field Name="**Player**" Value="Steam Name: **[{name}](https://steamcommunity.com/profiles/{steam_id})** ({steam_id}) &#xA; Character Name: **{charactername}** " Inline="true" />
            <Field Name="**Command**" Value="Command: `{command}` &#xA;" Inline="true" />
            <Field Name="**Cancelled**" Value="`{cancelled}` &#xA;" Inline="true" />
          </Fields>
          <Footer Text="{server_name}" />
          <WithCurrentTimestamp>true</WithCurrentTimestamp>
          <ColorHex>#ff0000</ColorHex>
        </Embed>
      </Embeds>
    </DutyCommands>
  </Discord>
</DutyConfiguration>
```


