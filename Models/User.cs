﻿using System;
using System.Collections.Generic;

namespace Kliker.Models;

/// <summary>
/// Zawiera informacje o kontach użytkowników. Każdy rekord to reprezentacja jednego użytkownika w postaci loginu, maila, hasła...
/// </summary>
public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public int Lvl { get; set; }

    public bool Banned { get; set; }

    public int Points { get; set; } = 0;

    public virtual ICollection<PlayersAchievement> PlayersAchievements { get; set; } = [];

    public virtual ICollection<PlayersUpgrade> PlayersUpgrades { get; set; } = [];

    public User(string username, string email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Lvl = 0;
        Banned = false;
        Points = 0;
    }
}