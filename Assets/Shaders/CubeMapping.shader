Shader "Custom/CubeMapping" {
   Properties {
      _Cube("Reflection Map", Cube) = "" {}
   }
   SubShader {
      Pass {   
         GLSLPROGRAM

         uniform samplerCube _Cube;   

         uniform vec3 _WorldSpaceCameraPos; 
         uniform mat4 _Object2World; // model matrix
         uniform mat4 _World2Object; // inverse model matrix
 
         varying vec3 normalDirection;
         varying vec3 viewDirection;
 
         #ifdef VERTEX
 
         void main()
         {            
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object;
 
            normalDirection = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            viewDirection = vec3(modelMatrix * gl_Vertex 
               - vec4(_WorldSpaceCameraPos, 1.0));
 
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
 
         #ifdef FRAGMENT
 
         void main()
         {
            vec3 reflectedDirection = 
               reflect(viewDirection, normalize(normalDirection));
            gl_FragColor = textureCube(_Cube, reflectedDirection);
         }
 
         #endif
 
         ENDGLSL
      }
   }
}
