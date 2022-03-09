/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2020.                                   *
 *                                                                            *
 * Use subject to the terms of the Apache License 2.0 available at            *
 * http://www.apache.org/licenses/LICENSE-2.0, or another agreement           *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using UnityEditor;

namespace Leap.Unity.GraphicalRenderer {

  [CustomPropertyDrawer(typeof(LeapBlendShapeData))]
  public class LeapBlendShapeDataDrawer : CustomPropertyDrawerBase {

    protected override void init(SerializedProperty property) {
      base.init(property);

      drawProperty("_amount");
      drawProperty("_type");

      var typeProp = property.FindPropertyRelative("_type");

      drawConditionalType("_translation", typeProp, LeapBlendShapeData.BlendShapeType.Translation);
      drawConditionalType("_rotation", typeProp, LeapBlendShapeData.BlendShapeType.Rotation);
      drawConditionalType("_scale", typeProp, LeapBlendShapeData.BlendShapeType.Scale);
      drawConditionalType("_transform", typeProp, LeapBlendShapeData.BlendShapeType.Transform);
    }

    private void drawConditionalType(string name, SerializedProperty typeProp, LeapBlendShapeData.BlendShapeType type) {
      drawPropertyConditionally(name, () => {
        return !typeProp.hasMultipleDifferentValues && typeProp.intValue == (int)type;
      });
    }
  }
}
