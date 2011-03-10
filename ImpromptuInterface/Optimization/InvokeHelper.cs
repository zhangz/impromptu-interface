﻿// 
//  Copyright 2011 Ekon Benefits
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImpromptuInterface.Build;
using Microsoft.CSharp.RuntimeBinder;

namespace ImpromptuInterface.Optimization
{
    internal class DummmyNull
    {
        
    }

    internal static class InvokeHelper
    {
        internal static readonly Type[] FuncKinds;
        internal static readonly Type[] ActionKinds;
        static InvokeHelper()
        {
            FuncKinds = new []
                            {
                                typeof(Func<>), //0
                                typeof(Func<,>),
                                typeof(Func<,,>),
                                typeof(Func<,,,>),
                                typeof(Func<,,,,>),
                                typeof(Func<,,,,,>),
                                typeof(Func<,,,,,,>),
                                typeof(Func<,,,,,,,>),
                                typeof(Func<,,,,,,,,>),
                                typeof(Func<,,,,,,,,,>),
                                typeof(Func<,,,,,,,,,,>),
                                typeof(Func<,,,,,,,,,,,>),
                                typeof(Func<,,,,,,,,,,,,>),
                                typeof(Func<,,,,,,,,,,,,,>),
                                typeof(Func<,,,,,,,,,,,,,,>),
                                typeof(Func<,,,,,,,,,,,,,,,>),
                                typeof(Func<,,,,,,,,,,,,,,,,>), //16
                            };
            ActionKinds = new []
                            {
                                typeof(Action), //0
                                typeof(Action<>),
                                typeof(Action<,>),
                                typeof(Action<,,>),
                                typeof(Action<,,,>),
                                typeof(Action<,,,,>),
                                typeof(Action<,,,,,>),
                                typeof(Action<,,,,,,>),
                                typeof(Action<,,,,,,,>),
                                typeof(Action<,,,,,,,,>),
                                typeof(Action<,,,,,,,,,>),
                                typeof(Action<,,,,,,,,,,>),
                                typeof(Action<,,,,,,,,,,,>),
                                typeof(Action<,,,,,,,,,,,,>),
                                typeof(Action<,,,,,,,,,,,,,>),
                                typeof(Action<,,,,,,,,,,,,,,>),
                                typeof(Action<,,,,,,,,,,,,,,,>), //16
                            };
        }

        #region InvokeMemberAction Optimizations
  

        internal static void InvokeMemberAction(CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    bool moreDynamic,
                                                    object target, params dynamic [] args)
        {
          
            var tSwitch = args.Length;
            switch (tSwitch)
            {
                case 0:
                    InvokeMemberActionHelper(binder, name, context, target);
                    break;
                case 1:
                    if (moreDynamic)
                    {
                        InvokeHelper.InvokeMemberActionHelper<dynamic>(binder, name, context, target, args[0]);
                        break;
                    }

                    InvokeHelper.InvokeMemberActionHelper(binder, name, context, target, args[0]);
                    break;
                case 2:
                    if (moreDynamic)
                    {
                        InvokeHelper.InvokeMemberActionHelper<dynamic,dynamic>(binder, name, context, target, args[0], args[1]);
                        break;
                    }
                    InvokeHelper.InvokeMemberActionHelper(binder, name, context, target, args[0], args[1]);
                    break;
                case 3:
                    if (moreDynamic)
                    {
                        InvokeHelper.InvokeMemberActionHelper<dynamic, dynamic,dynamic>(binder, name, context, target, args[0], args[1], args[2]);
                        break;
                    }
                    InvokeHelper.InvokeMemberActionHelper(binder, name, context, target, args[0], args[1], args[2]);
                    break;
                case 4:
                    if (moreDynamic)
                    {
                        InvokeHelper.InvokeMemberActionHelper<dynamic, dynamic, dynamic, dynamic>(binder, name, context, target, args[0], args[1], args[2], args[3]);
                        break;
                    }
                    InvokeHelper.InvokeMemberActionHelper(binder, name, context, target, args[0], args[1], args[2], args[3]);
                    break;
                case 5:
                    if (moreDynamic)
                    {
                        InvokeHelper.InvokeMemberActionHelper<dynamic, dynamic, dynamic, dynamic,dynamic>(binder, name, context, target, args[0], args[1], args[2], args[3], args[4]);
                        break;
                    }
                    InvokeHelper.InvokeMemberActionHelper(binder, name, context, target, args[0], args[1], args[2], args[3], args[4]);
                    break;
                default:
                    var tArgTypes = args.Select(it => moreDynamic ? typeof(object) : ((object)it).GetType());
                    var tDelagateType = BuildProxy.GenerateCallSiteFuncType(tArgTypes, typeof(void));
                    Impromptu.Invoke(Impromptu.CreateCallSite(tDelagateType, binder, name, context), target, args);
                    break;

            }
        }

        private static void InvokeMemberActionHelper(
                                                    CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    object target)
        {

            var tCallSite = Impromptu.CreateCallSite<Action<CallSite, object>>(binder, name, context);
            tCallSite.Target.Invoke(tCallSite, target);
        }
        private static void InvokeMemberActionHelper<T1>(
                                                    CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    object target, T1 arg1)
        {

            var tCallSite = Impromptu.CreateCallSite<Action<CallSite, object, T1>>(binder, name, context);
            tCallSite.Target.Invoke(tCallSite, target, arg1);
        }
        private static void InvokeMemberActionHelper<T1, T2>(
                                                    CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    object target, T1 arg1, T2 arg2)
        {

            var tCallSite = Impromptu.CreateCallSite<Action<CallSite, object, T1, T2>>(binder, name, context);
            tCallSite.Target.Invoke(tCallSite, target, arg1, arg2);
        }
        private static void InvokeMemberActionHelper<T1, T2, T3>(
                                                    CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    object target, T1 arg1, T2 arg2, T3 arg3)
        {

            var tCallSite = Impromptu.CreateCallSite<Action<CallSite, object, T1, T2, T3>>(binder, name, context);
            tCallSite.Target.Invoke(tCallSite, target, arg1, arg2, arg3);
        }


        private static void InvokeMemberActionHelper<T1, T2, T3,T4>(
                                                CallSiteBinder binder,
                                                string name,
                                                Type context,
                                                object target, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {

            var tCallSite = Impromptu.CreateCallSite<Action<CallSite, object, T1, T2, T3, T4>>(binder, name, context);
            tCallSite.Target.Invoke(tCallSite, target, arg1, arg2, arg3, arg4);
        }

        private static void InvokeMemberActionHelper<T1, T2, T3, T4, T5>(
                                            CallSiteBinder binder,
                                            string name,
                                            Type context,
                                            object target, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {

            var tCallSite = Impromptu.CreateCallSite<Action<CallSite, object, T1, T2, T3, T4, T5>>(binder, name, context);
            tCallSite.Target.Invoke(tCallSite, target, arg1, arg2, arg3, arg4, arg5);
        }


        #endregion

       
        #region InvokeMember Optimizations


        internal static object InvokeMember(CallSiteBinder binder,
                                       string name,
                                     Type context,
            bool moreDynamic,
                                     object target, params dynamic [] args)
        {
           
            var tSwitch = args.Length;
    
            switch (tSwitch)
            {
                case 0:
                    return InvokeMemberHelper(binder, name, context, target);
                case 1:
                    if (moreDynamic)
                    {
                        return InvokeHelper.InvokeMemberHelper<dynamic>(binder, name, context, target, args[0]);
                    }
                    return InvokeHelper.InvokeMemberHelper(binder, name, context, target, args[0]);
                case 2:
                    if (moreDynamic)
                    {
                        return InvokeHelper.InvokeMemberHelper<dynamic,dynamic>(binder, name, context, target, args[0], args[1]);
                    }
                    return InvokeHelper.InvokeMemberHelper(binder, name, context, target, args[0], args[1]);
                case 3:
                    if (moreDynamic)
                    {
                        return InvokeHelper.InvokeMemberHelper<dynamic, dynamic, dynamic>(binder, name, context, target, args[0], args[1], args[2]);
                    }
                    return InvokeHelper.InvokeMemberHelper(binder, name, context, target, args[0], args[1], args[2]);
                case 4:
                    if (moreDynamic)
                    {
                        return InvokeHelper.InvokeMemberHelper<dynamic, dynamic, dynamic,dynamic>(binder, name, context, target, args[0], args[1], args[2], args[3]);
                    }
                    return InvokeHelper.InvokeMemberHelper(binder, name, context, target, args[0], args[1], args[2], args[3]);
                case 5:
                    if (moreDynamic)
                    {
                        return InvokeHelper.InvokeMemberHelper<dynamic, dynamic, dynamic, dynamic,dynamic>(binder, name, context, target, args[0], args[1], args[2], args[3], args[4]);
                    }
                    return InvokeHelper.InvokeMemberHelper(binder, name, context, target, args[0], args[1], args[2], args[3], args[4]);
                default:
                    var tArgTypes = args.Select(it => moreDynamic ? typeof(object) : ((object)it).GetType());
                    var tDelagateType = BuildProxy.GenerateCallSiteFuncType(tArgTypes, typeof(object));
                    return Impromptu.Invoke(Impromptu.CreateCallSite(tDelagateType, binder, name, context), target, args);

            }
        }


        private static object InvokeMemberHelper(
                                                    CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    object target)
        {

            var tCallSite = Impromptu.CreateCallSite<Func<CallSite, object, object>>(binder, name, context);
            return tCallSite.Target.Invoke(tCallSite, target);
        }

      
        private static object InvokeMemberHelper<T1>(
                                                    CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    object target, T1 arg1)
        {

            var tCallSite = Impromptu.CreateCallSite<Func<CallSite, object, T1, object>>(binder, name, context);
            return tCallSite.Target.Invoke(tCallSite, target, arg1);
        }
        private static object InvokeMemberHelper<T1, T2>(
                                                    CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    object target, T1 arg1, T2 arg2)
        {

            var tCallSite = Impromptu.CreateCallSite<Func<CallSite, object, T1, T2, object>>(binder, name, context);
            return tCallSite.Target.Invoke(tCallSite, target, arg1, arg2);
        }


        private static object InvokeMemberHelper<T1, T2, T3>(
                                                    CallSiteBinder binder,
                                                    string name,
                                                    Type context,
                                                    object target, T1 arg1, T2 arg2, T3 arg3)
        {

            var tCallSite = Impromptu.CreateCallSite<Func<CallSite, object, T1, T2, T3, object>>(binder, name, context);
            return tCallSite.Target.Invoke(tCallSite, target, arg1, arg2, arg3);
        }

        private static object InvokeMemberHelper<T1, T2, T3, T4>(
                                               CallSiteBinder binder,
                                               string name,
                                               Type context,
                                               object target, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {

            var tCallSite = Impromptu.CreateCallSite<Func<CallSite, object, T1, T2, T3, T4, object>>(binder, name, context);
            return tCallSite.Target.Invoke(tCallSite, target, arg1, arg2, arg3, arg4);
        }

        private static object InvokeMemberHelper<T1, T2, T3, T4, T5>(
                                            CallSiteBinder binder,
                                            string name,
                                            Type context,
                                            object target, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {

            var tCallSite = Impromptu.CreateCallSite<Func<CallSite, object, T1, T2, T3, T4,T5, object>>(binder, name, context);
            return tCallSite.Target.Invoke(tCallSite, target, arg1, arg2, arg3, arg4,arg5);
        }

        #endregion


        internal static object FastDynamicInvokeReturn(Delegate del, dynamic [] args)
        {
            dynamic tDel =del;
            switch(args.Length){
                default:
                    return del.DynamicInvoke(args);
                    #region Optimization
                case 1:
                    return tDel(args[0]);
                case 2:
                    return tDel(args[0], args[1]);
                case 3:
                    return tDel(args[0], args[1], args[2]);
                case 4:
                    return tDel(args[0], args[1], args[2], args[3]);
                case 5:
                    return tDel(args[0], args[1], args[2], args[3], args[4]);
                case 6:
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5]);
                case 7:
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                case 8:
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                case 9:
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                case 10:
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                case 11:
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                case 12:
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                case 13:
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                case 14:                                                                                                                    
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                case 15:                                                                                                                  
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                case 16:                                                                                                                    
                    return tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                    #endregion
            }
        }

        internal static void FastDynamicInvokeAction(Delegate del, params dynamic [] args)
        {
            dynamic tDel =del;
            switch(args.Length){
                default:
                    del.DynamicInvoke(args);
                    return;
                #region Optimization
                case 1:
                    tDel(args[0]);
                    return;
                case 2:
                    tDel(args[0], args[1]);
                    return;
                case 3:
                    tDel(args[0], args[1], args[2]);
                    return;
                case 4:
                    tDel(args[0], args[1], args[2], args[3]);
                    return;
                case 5:
                    tDel(args[0], args[1], args[2], args[3], args[4]);
                    return;
                case 6:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5]);
                    return;
                case 7:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    return;
                case 8:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    return;
                case 9:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    return;
                case 10:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                    return;
                case 11:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                    return;
                case 12:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                    return;
                case 13:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                    return;
                case 14:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                    return;
                case 15:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                    return;
                case 16:
                    tDel(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                    return;
                    #endregion
            }
        }
    }
}
