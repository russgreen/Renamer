﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Renamer;
internal static class DispatcherHelper
{
    /// <summary>
    /// Simulate Application.DoEvents function of <see cref=" System.Windows.Forms.Application"/> class.
    /// </summary>
    internal static void DoEvents()
    {
        DispatcherFrame frame = new();
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
            new DispatcherOperationCallback(ExitFrames), frame);

        try
        {
            Dispatcher.PushFrame(frame);
        }
        catch (InvalidOperationException)
        {
        }
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    private static object ExitFrames(object frame)
    {
        ((DispatcherFrame)frame).Continue = false;

        return null;
    }
}
