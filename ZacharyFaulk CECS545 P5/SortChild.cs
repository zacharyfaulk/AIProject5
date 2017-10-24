using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P5
{
    class SortChild
    {
        //Sorts the newChild object according to its distance from smallest distance to largest distance
        //The List only contains the up to the top "globalVars.maxChildren" children
        public static void sortChild(newChild tempChild, ref globalVars globalVars, ref List<newChild> childList)
        {
            //If the list is empty, add the current child
            if (childList.Count == 0)
            {
                childList.Add(tempChild);
            }
            //If the list is not empty
            else
            {
                //If the list is currently full
                if (childList.Count == globalVars.maxChildren)
                {
                    //If the distance of the last child in the list is less than the current child's distance,
                    //don't sort the current child and return back to the NewGen class
                    if (childList[globalVars.maxChildren - 1].distance < tempChild.distance)
                    {
                        return;
                    }
                    //If the distance of the last child in the list is greater than the current child's distance,
                    //sort the current child as it is guarenteed to find a spot in the list
                    else
                    {
                        //Check all the distances in the list and find where
                        //the current child should be inserted by finding the 
                        //first element in the list that has a distance greater than
                        //the current child and insert the child before that element
                        for (int c = 0; c < childList.Count; c++)
                        {
                            if (tempChild.distance < childList[c].distance)
                            {
                                childList.Insert(c, tempChild);
                                childList.RemoveAt(globalVars.maxChildren);
                                return;
                            }
                        }
                    }
                }
                //If the list is not full,
                //sort the current child as it is guarenteed to find a spot in the list
                else
                {
                    //Check all the distances in the list and find where
                    //the current child should be inserted by finding the 
                    //first element in the list that has a distance greater than
                    //the current child and insert the child before that element
                    for (int c = 0; c < childList.Count; c++)
                    {
                        if (tempChild.distance < childList[c].distance)
                        {
                            childList.Insert(c, tempChild);
                            return;
                        }
                        //If the current child needs to be the
                        //last item in the list for the first time
                        else if(c == childList.Count - 1)
                        {
                            childList.Add(tempChild);
                            return;
                        }
                    }
                }
            }
        }
    }
}
