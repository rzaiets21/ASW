using System;

namespace Core.Mediation
{
    public interface IMediatorBindingInfo
    {
        Type ViewType { get; }
        Type MediatorType { get; }
    }
}