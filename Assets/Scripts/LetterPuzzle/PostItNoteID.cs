// Ooga Booga Unity! We tell Unity to wake up and play with us!
// Big fun box is UnityEngine, packed with sparkly toy tools!
// We call this magic stuff so our sticky notes can dance!
using UnityEngine;

// Boop beep! We make this PostItNoteID super squishy!
// Unity gobbles it up so Inspector can show its guts!
// Squishable class means we poke and prod it in the Editor!
[System.Serializable]
// Here comes the PostItNoteID tribe!
// Slap it on GameObjects so they become sticky note people!
// Each one shouts "I'm sticky! Pick me!"
public class PostItNoteID : MonoBehaviour
{
    // ID number pow! Sticky goes "I am number 1!"
    // Pick a number like you pick candy, sweet!
    // Unity learns this number and never forgets it!
    public int ID;

    // isOrange is boop boop true-false toggle!
    // True means orange splash, false means not-orange dash!
    // Flip it like a pancake in the Inspector pan!
    public bool isOrange;

    // Letter time! Our sticky sings a letter tune!
    // Dropdown sparkles so we only pick real letters!
    // Keeps our puzzle neat like lined paper!
    public PostItNoteLetter letter;

    // Enum party! These are the only letters allowed!
    // No scribbles outside the lines, only menu picks!
    // We list them so Unity says "Yes, these are good!"
    public enum PostItNoteLetter
    {
        // D is for Dum, Doodle, Dance!
        D,

        // E is for Eureka, Elephant, Eeek!
        E,

        // L is for Lollipop, Loopy, LOL!
        L,

        // T is for T-Rex, Tada, Tango!
        T,

        // I is for Ice, Igloo, Icky!
        I,

        // O is for Ooga, Booga, Oink!
        O,

        // N is for Ninja, Nap, Noodle!
        N,

        // C is for Cookie, Crash, Cackle!
        C,

        // G is for Giggle, Gobble, Gooo!
        G,
    }
}
