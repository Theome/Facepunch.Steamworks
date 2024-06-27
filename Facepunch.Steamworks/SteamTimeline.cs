using System;
using Steamworks.Data;

namespace Steamworks;

public class SteamTimeline : SteamClientClass<SteamTimeline>
{
	internal static ISteamTimeline Internal => Interface as ISteamTimeline;

	internal override bool InitializeInterface( bool server )
	{
		SetInterface( server, new ISteamTimeline( server ) );
		if ( Interface.Self == IntPtr.Zero ) return false;
		
		return true;
	}

	/// <summary>
	/// Sets a description for the current game state in the timeline. These help the user to find specific moments in the timeline when saving clips. Setting a new state description replaces any previous description.
	/// </summary>
	/// <param name="description">A localized string in the language returned by SteamUtils()->GetSteamUILanguage()</param>
	/// <param name="timeDelta">The time offset in seconds to apply to this state change. Negative times indicate an event that happened in the past.</param>
	public static void SetTimelineStateDescription( string description, float timeDelta ) => Internal.SetTimelineStateDescription( description, timeDelta );

	/// <summary>
	/// Clears the previous set game state in the timeline.
	/// </summary>
	/// <param name="timeDelta">The time offset in seconds to apply to this state change. Negative times indicate an event that happened in the past.</param>
	public static void ClearTimelineStateDescription( float timeDelta ) => Internal.ClearTimelineStateDescription( timeDelta );

	/// <summary>
	/// Use this to mark an event on the Timeline. The event can be instantaneous or take some amount of time to complete, depending on the value passed in flDurationSeconds.
	/// </summary>
	/// <param name="icon">The name of the icon to show at the timeline at this point. This can be one of the icons uploaded through the Steamworks partner Site for your title, or one of the provided icons that start with steam_</param>
	/// <param name="title"></param>
	/// <param name="description"></param>
	/// <param name="priority">Provide the priority to use when the UI is deciding which icons to display in crowded parts of the timeline. Events with larger priority values will be displayed more prominently than events with smaller priority values. This value must be between 0 and k_unMaxTimelinePriority</param>
	/// <param name="startOffsetInSeconds">The time offset in seconds to apply to the start of the event. Negative times indicate an event that happened in the past.
	/// One use of this parameter is to handle events whose significance is not clear until after the fact. For instance if the player starts a damage over time effect on another player, which kills them 3.5 seconds later, the game could pass -3.5 as the start offset and cause the event to appear in the timeline where the effect started.</param>
	/// <param name="durationInSeconds">The duration of the event, in seconds. Pass 0 for instantaneous events.</param>
	/// <param name="possibleClip">Allows the game to describe events that should be suggested to the user as possible video clips.</param>
	public static void AddTimelineEvent(string icon, string title, string description, uint priority, float startOffsetInSeconds, float durationInSeconds, TimelineEventClipPriority possibleClip) => Internal.AddTimelineEvent(icon, title, description, priority, startOffsetInSeconds, durationInSeconds, possibleClip);
	
	/// <summary>
	/// hanges the color of the timeline bar. See ETimelineGameMode for how to use each value.
	/// </summary>
	/// <param name="eMode"></param>
	public static void SetTimelineGameMode(TimelineGameMode eMode) => Internal.SetTimelineGameMode( eMode );
}
