Shader "Custom/BumpMapping" {
   Properties {
      _MainTex ("Texture Image", 2D) = "white" {} 
      _BumpMap ("Normal Map", 2D) = "bump" {}
      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
      _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
      _Shininess ("Shininess", Float) = 10
   }
   SubShader {
      Pass {      
         Tags { "LightMode" = "ForwardBase" } 
            //ambient light + first light source
 
         GLSLPROGRAM

		 uniform sampler2D _MainTex;
         uniform sampler2D _BumpMap;	
         uniform vec4 _BumpMap_ST;
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _Shininess;

         uniform vec3 _WorldSpaceCameraPos; 

         uniform mat4 _Object2World;
         uniform mat4 _World2Object;
         uniform vec4 _WorldSpaceLightPos0; 

         uniform vec4 _LightColor0; 

 
         varying vec4 position; 

         varying vec4 textureCoordinates; 
         varying mat3 localSurface2World;
 
         #ifdef VERTEX
 
         attribute vec4 Tangent;
 
         void main()
         {                                
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object;
 
            localSurface2World[0] = normalize(vec3(
               modelMatrix * vec4(vec3(Tangent), 0.0)));
            localSurface2World[2] = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            localSurface2World[1] = normalize(
               cross(localSurface2World[2], localSurface2World[0]) 
               * Tangent.w);
 
            position = modelMatrix * gl_Vertex;
            textureCoordinates = gl_MultiTexCoord0;
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {
            vec4 encodedNormal = texture2D(_BumpMap, 
               _BumpMap_ST.xy * textureCoordinates.xy 
               + _BumpMap_ST.zw);
            vec3 localCoords = 
               vec3(2.0 * encodedNormal.ag - vec2(1.0), 0.0);
            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));
               // approximation without sqrt: localCoords.z = 
               // 1.0 - 0.5 * dot(localCoords, localCoords);
            vec3 normalDirection = 
               normalize(localSurface2World * localCoords);
 
            vec3 viewDirection = 
               normalize(_WorldSpaceCameraPos - vec3(position));
            vec3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w)
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - position);
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance;
               lightDirection = normalize(vertexToLightSource);
            }
 
            vec3 ambientLighting = 
               vec3(gl_LightModel.ambient) * vec3(_Color);
 
            vec3 diffuseReflection = 
               attenuation * vec3(_LightColor0) * vec3(_Color) 
               * max(0.0, dot(normalDirection, lightDirection));
 
            vec3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
            {
               specularReflection = vec3(0.0, 0.0, 0.0); 
            }
            else
            {
               specularReflection = attenuation * vec3(_LightColor0) 
                  * vec3(_SpecColor) * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
 
            gl_FragColor = texture2D(_MainTex, vec2(textureCoordinates)) * vec4(ambientLighting 
               + diffuseReflection + specularReflection, 1.0);
         }
 
         #endif
 
         ENDGLSL
      }
 
      Pass {      
         Tags { "LightMode" = "ForwardAdd" } 
            // pass for additional light sources
         Blend One One // additive blending 
 
        GLSLPROGRAM

		 uniform sampler2D _MainTex;
         uniform sampler2D _BumpMap;	
         uniform vec4 _BumpMap_ST;
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _Shininess;

         uniform vec3 _WorldSpaceCameraPos; 

         uniform mat4 _Object2World;
         uniform mat4 _World2Object; 
         uniform vec4 _WorldSpaceLightPos0; 

         uniform vec4 _LightColor0; 

 
         varying vec4 position; 
         varying vec4 textureCoordinates; 
         varying mat3 localSurface2World; // mapping from 
 
         #ifdef VERTEX
 
         attribute vec4 Tangent;
 
         void main()
         {                                
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object;
 
            localSurface2World[0] = normalize(vec3(
               modelMatrix * vec4(vec3(Tangent), 0.0)));
            localSurface2World[2] = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            localSurface2World[1] = normalize(
               cross(localSurface2World[2], localSurface2World[0]) 
               * Tangent.w);
 
            position = modelMatrix * gl_Vertex;
            textureCoordinates = gl_MultiTexCoord0;
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {
            vec4 encodedNormal = texture2D(_BumpMap, 
               _BumpMap_ST.xy * textureCoordinates.xy 
               + _BumpMap_ST.zw);
            vec3 localCoords = 
               vec3(2.0 * encodedNormal.ag - vec2(1.0), 0.0);
            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));

            vec3 normalDirection = 
               normalize(localSurface2World * localCoords);
 
            vec3 viewDirection = 
               normalize(_WorldSpaceCameraPos - vec3(position));
            vec3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light
            {
               attenuation = 1.0;
               lightDirection = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - position);
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance;
               lightDirection = normalize(vertexToLightSource);
            }
 
            vec3 diffuseReflection = 
               attenuation * vec3(_LightColor0) * vec3(_Color) 
               * max(0.0, dot(normalDirection, lightDirection));
 
            vec3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
            {
               specularReflection = vec3(0.0, 0.0, 0.0); 
            }
            else
            {
               specularReflection = attenuation * vec3(_LightColor0) 
                  * vec3(_SpecColor) * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
 
            gl_FragColor = texture2D(_MainTex, vec2(textureCoordinates)) *
               vec4(diffuseReflection + specularReflection, 1.0);
         }
 
         #endif
 
         ENDGLSL
      }
   } 

}

