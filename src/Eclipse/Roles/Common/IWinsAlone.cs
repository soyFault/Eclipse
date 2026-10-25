namespace Eclipse.Roles;

// Marks a role that wins on its own: whoever played it is taken out of the list of winners of the other endings (a dead Jester is not a winner of the
// crew), and the role's own win puts them back.
internal interface IWinsAlone
{
}
